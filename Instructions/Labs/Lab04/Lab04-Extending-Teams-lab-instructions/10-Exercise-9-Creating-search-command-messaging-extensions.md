# Exercise 9: Creating search command messaging extensions

In this exercise, you’ll learn how to execute a messaging extension search command from an existing message.

> **IMPORTANT:**
> This exercise assumes you have created the Microsoft Teams app project with the Yeoman generator.

## Task 1: Add a new search messaging extension to the Teams app

In a previous exercise, you created an action messaging extension that enabled a user to add the details of a planet to a message.

In this section, you'll add a search messaging extension to find a specific planet.

### Update the app's configuration

1. First, update app's manifest to add the new messaging extension. Locate and open the **./src/manifest/manifest.json** file.

1. You must increment the version of the app to upgrade an existing installed version. Locate the property `version` and increment the version to `1.0.1`.

1. Next, locate the `composeExtensions.commands` array. Add the following object to the array to add the search extension:

    ```json
    {
      "id": "planetExpanderSearch",
      "type": "query",
      "title": "Planet Lookup",
      "description": "Search for a planet.",
      "context": ["compose"],
      "parameters": [{
        "name": "searchKeyword",
        "description": "Enter 'inner','outer' or the name of a specific planet",
        "title": "Planet"
      }]
    }
    ```

### Update the bot code

The next step is to update the bot's code.

1. Locate and open the bot in the file **./src/server/planetBot/planetBot.ts**.

1. Update the `import` statement for the **botbuilder** package to include the objects `MessagingExtensionQuery` and `MessagingExtensionResponse`:

    ```typescript
    import {
      TeamsActivityHandler,
      TurnContext,
      MessageFactory,
      CardFactory, MessagingExtensionAction, MessagingExtensionActionResponse, MessagingExtensionAttachment,
      MessagingExtensionQuery, MessagingExtensionResponse
    } from "botbuilder";
    ```

1. Next, add the following method to the `PlanetBot` class:

    ```typescript
    protected handleTeamsMessagingExtensionQuery(context: TurnContext, query: MessagingExtensionQuery): Promise<MessagingExtensionResponse> {
      // get the search query
      let searchQuery = "";
      if (query && query.parameters && query.parameters[0].name === "searchKeyword" && query.parameters[0].value) {
        searchQuery = query.parameters[0].value.trim().toLowerCase();
      }

      // load planets
      const planets: any = require("./planets.json");
      // search results
      let queryResults: string[] = [];

      switch (searchQuery) {
        case "inner":
          // get all planets inside asteroid belt
          queryResults = planets.filter((planet) => planet.id <= 4);
          break;
        case "outer":
          // get all planets outside asteroid belt
          queryResults = planets.filter((planet) => planet.id > 4);
          break;
        default:
          // get the specified planet
          queryResults.push(planets.filter((planet) => planet.name.toLowerCase() === searchQuery)[0]);
      }

      // get the results as cards
      const searchResultsCards: MessagingExtensionAttachment[] = [];
      queryResults.forEach((planet) => {
        searchResultsCards.push(this.getPlanetResultCard(planet));
      });

      const response: MessagingExtensionResponse = {
        composeExtension: {
          type: "result",
          attachmentLayout: "list",
          attachments: searchResultsCards
        }
      } as MessagingExtensionResponse;

      return Promise.resolve(response);
    }
    ```

    This method will first get the search keyword from the query sent to the bot from Microsoft Teams. It then will retrieve planets based on three different queries:

    - **inner**: this will return all the planets inside the asteroid belt (*Mercury to Mars*)
    - **outer**: this will return all planets outside the asteroid belt (*Jupiter to Neptune*)
    - *keyword*: this will retrieve the specific planet entered

    It will then take the query results, convert them to cards and add them to the `MessagingExtensionResponse` returned to the Bot Framework and ultimately to Microsoft Teams.

1. Lastly, add the following utility method to the `PlanetBot` class to create the card for each search result:

    ```typescript
    private getPlanetResultCard(selectedPlanet: any): MessagingExtensionAttachment {
      return CardFactory.heroCard(selectedPlanet.name, selectedPlanet.summary, [selectedPlanet.imageLink]);
    }
    ```

### Test the updated messaging extension

1. From the command line, navigate to the root folder for the project and execute the following command:

    ```console
    gulp ngrok-serve
    ```

    > **IMPORTANT:**
    > Recall from a previous exercise, Ngrok will create a new subdomain. You need to update your bot registration's **Messaging endpoint** in the Azure portal (*shown in a previous exercise*) with this new domain before testing it.

1. First, update the existing installed version of the bot.

1. In the browser, navigate to **https://teams.microsoft.com** and sign in with the credentials of a Work and School account.

1. Using the app bar navigation menu, select the **More added apps** button. Then select **Browse all apps**, select the menu in the top-right corner of the **Planet Messaging** and select **Update**.

    ![Screenshot of updating an installed Microsoft Teams app](../../Linked_Image_Files/Messaging_Extensions/05-test-01.png)

1. When prompted, select the updated package of the Microsoft Teams app. Microsoft Teams will update the app to the new version.

1. After updating the app, go back to the 1:1 chat where you tested the messaging extension in the previous exercise. Select the **Planet Messaging** icon below the compose message box in the chat. This will now present the search experience.

1. Enter the string **outer** in the search box and wait a few seconds. Microsoft Teams will execute the search and return the results:

    ![Screenshot of a search messaging extension](../../Linked_Image_Files/Messaging_Extensions/05-test-02.png)

## Summary

In this exercise, you learned how to execute a messaging extension search command from an existing message.
