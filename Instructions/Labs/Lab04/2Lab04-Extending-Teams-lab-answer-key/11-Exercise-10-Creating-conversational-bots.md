# Exercise 10: Creating conversational bots

In this exercise, you’ll learn how to create and add a new bot to a Microsoft Teams app and interact with it from the Microsoft Teams client.

> **NOTE:**
> This exercise requires a valid Azure subscription in order to create a bot using Bot Framework. However, if you do not have an Azure subscription, you can use the legacy Bot Framework Registration Portal. Refer to the following docs for more information: [Create a bot for Microsoft Teams](/microsoftteams/platform/bots/how-to/create-a-bot-for-teams)


## Task 1: Register a new bot in Microsoft Azure

The first step is to create a new Microsoft Teams bot. Adding a bot to the Teams app involves two steps:

1. Register the bot with Microsoft Azure's Bot Framework
1. Add a bot to the project codebase

### Register the bot with Microsoft Azure's Bot Framework

1. Open a browser and navigate to the [Azure portal](https://portal.azure.com). Sign in using a **Work or School Account** that has rights to create resources in your Azure subscription.

1. Select **Create a resource** in the left-hand navigation:

    ![Screenshot of the primary Azure navigation](../../Linked_Image_Files/Bots/03-azure-portal-01.png)

1. Enter **resource group** in the **Search the marketplace** input box, and select **Resource group**.

    ![Screenshot of creating a resource group - create a resource menu item](../../Linked_Image_Files/Bots/03-azure-portal-02.png)

1. On the **Resource Group** page, select the **Create** button to create a new resource group.

1. Select a valid subscription, enter a name for the resource group, and select the wanted region. _None of these choices will impact the bot registration and are up to you._

    ![Screenshot of creating a resource group - search for the resource group option](../../Linked_Image_Files/Bots/03-azure-portal-03.png)

1. Complete the wizard to create the resource group. Once Azure has completed the resource group creation process, navigate to the resource group.

1. From the resource group, select the **Add** or **Create resources** button.

    ![Screenshot of creating a new resource](../../Linked_Image_Files/Bots/03-azure-bot-registration-01.png)

1. Enter **bot** in the **Search the marketplace** input box, and select **Bot Channels Registration** from the list of resources returned. Then select **Create** on the next page to start the process of registering a new bot resource:

    ![Screenshot of searching for the bot registration resource](../../Linked_Image_Files/Bots/03-azure-bot-registration-02.png)

1. In the **Bot Channels Registration** blade, enter the following values and then select **Create**:

    - **Bot handle**: _Enter a globally unique name for the bot_
    - **Subscription**: _Select the subscription you selected previously when creating the resource group_
    - **Resource group**: _Select the resource group you created previously_
    - **Location**: _Select your preferred Azure region_
    - **Pricing tier**: _Select a preferred pricing tier; the F0 tier is free_
    - **Messaging endpoint**: https://REPLACE_THIS.ngrok.io/api/messages

      > The bot registration needs to know the endpoint of the web service where the bot is implemented. This will change each time you start the ngrok utility used in previous exercises.

    - **Application Insights**: Off
    - **Microsoft App ID and password**: Auto create App ID and password

    Azure will start to provision the new resource. This will take a moment or two. Once it's finished, navigate to the bot resource in the resource group.

    ![Screenshot of the created bot channels registration resource](../../Linked_Image_Files/Bots/03-azure-bot-registration-03.png)

### Enable the Microsoft Teams channel for the bot

1. In order for the bot to interact with Microsoft Teams, you must enable the Teams channel.

1. From the bot resource in Azure, select **Channels** in the left-hand navigation.

1. On the **Connect to channels** pane, select the Microsoft Teams channel, then select **Save** to confirm the action.

    ![Screenshot enabling the Microsoft Teams channel](../../Linked_Image_Files/Bots/03-azure-bot-registration-04.png)

1. Once this process is complete, you should see both the **Web Chat** and **Microsoft Teams** listed in your enabled channels:

    ![Screenshot of the enabled bot channels](../../Linked_Image_Files/Bots/03-azure-bot-registration-05.png)

### Retrieve the bot app ID and password

When Azure created the bot, it also registered a new Azure AD app for the bot. Generate this new bot app a secret and copy the app's credentials.

1. Select **Configuration** from the left-hand navigation. Copy the **Microsoft App ID** of the bot as you'll need it later.

1. Select **Manage** to navigate to the Azure AD app blade:

    ![Screenshot of the bot's settings page](../../Linked_Image_Files/Bots/03-azure-bot-registration-06.png)

### Create a client secret for the app

In order for the daemon app to run without user involvement, it will sign in to Azure AD with an application ID and either a certificate or secret. In this exercise, you'll use a secret.

1. Select **Certificates & secrets** from the left-hand navigation panel.

1. Select the **New client secret** button:

    ![Screenshot of the Certificates & Secrets page in the Azure AD admin center](../../Linked_Image_Files/Bots/03-azure-bot-registration-07.png)

1. When prompted, give the secret a description and select one of the expiration duration options provided and select **Add**. _What you enter and select doesn't matter for the exercise._

1. The **Certificate & Secrets** page will display the new secret. It's important you copy this value as it's only shown this one time; if you leave the page and come back, it will only show as a masked value.

    ![Screenshot showing the new secret](../../Linked_Image_Files/Bots/03-azure-bot-registration-08.png)

1. Copy the value of the secret as you'll need it later.

## Task 2: Create Microsoft Teams app

In this section, you'll create a new Node.js project.

1. Open your command prompt, navigate to a directory where you want to save your work, create a new folder **learn-msteams-bots**, and change directory into that folder.

1. Run the Yeoman Generator for Microsoft Teams by running the following command:

    ```console
    yo teams
    ```

1. Yeoman will launch and ask you a series of questions. Answer the questions with the following values:

    - **What is your solution name?**: ConversationalBot
    - **Where do you want to place the files?**: Use the current folder
    - **Title of your Microsoft Teams App project?**: Conversational Bot
    - **Your (company) name? (max 32 characters)**: Contoso
    - **Which manifest version would you like to use?**: v1.8
    - **Quick scaffolding**: Yes
    - **What features do you want to add to your project?**: A bot
    - **The URL where you will host this solution?**: (Accept the default option)
    - **Would you like show a loading indicator when your app/tab loads?**: No
    - **What type of Bot would you like to use?** A new Bot Framework bot
    - **What is the name of your bot?** Conversational Bot
    - **What is the Microsoft App ID for the Bot. It's found in the Bot Framework portal (https://dev.botframework.com).** (Enter the application ID of the bot you created in the previous step)
    - **Do you want to add a static tab to your bot?** No
    - **Do you want to support file upload to the bot?** No
    - **Do you want to include bot calling support?** No

    > **NOTE:**
    > Most of the answers to these questions can be changed after creating the project. For example, the URL where the project will be hosted isn't important at the time of creating or testing the project.

    After answering the generator's questions, the generator will create the scaffolding for the project and then execute `npm install` that downloads all the dependencies required by the project.

### Update the default bot

The first version of this bot will respond to the message **MentionMe** in a 1:1 chat conversation. The response will mention the user who started the conversation.

1. To implement this functionality, locate and open the **./src/server/conversationalBot/ConversationalBot.ts** file and add the following method to the `ConversationalBot` class:

    ```typescript
    private async handleMessageMentionMeOneOnOne(context: TurnContext): Promise<void> {
      const mention = {
        mentioned: context.activity.from,
        text: `<at>${new TextEncoder().encode(context.activity.from.name)}</at>`,
        type: "mention"
      };

      const replyActivity = MessageFactory.text(`Hi ${mention.text} from a 1:1 chat.`);
      replyActivity.entities = [mention];
      await context.sendActivity(replyActivity);
    }
    ```

    The method handler you added contains a reference to two objects you haven't imported into the bot file.

1. Add the following code after the existing `import` statements at the top of the file to import the `TextEncoder` object:

    ```typescript
    import * as Util from "util";
    const TextEncoder = Util.TextEncoder;
    ```

1. Add a reference to the `MessageFactory` object, by adding it to the existing list of object references in the **botbuilder** package `import` at the top of the file:

    ```typescript
    import {
      StatePropertyAccessor,
      CardFactory,
      TurnContext,
      MemoryStorage,
      ConversationState,
      ActivityTypes,
      TeamsActivityHandler,
      MessageFactory,
    } from 'botbuilder';
    ```

1. Next, call this method when the bot receives the specific string **MentionMe** by doing the following:

    - Locate the handler `onMessage()` within the `constructor()`.
    - Locate and replace the line `if (text.startsWith("hello")) {` in the `onMessage()` handler with the following code:

      ```typescript
      if (text.startsWith("mentionme")) {
        await this.handleMessageMentionMeOneOnOne(context);
        return;
      } else if (text.startsWith("hello")) {
      ```

### Update the project's environment variables

The project contains a file used during development to set environment variables to store secure values, such as the Azure AD application's ID and secret that the bot is associated with.

You need to set these two values for the bot to work:

1. Locate and open the file **./.env**.
1. Locate the following section in the file, and set the values of the two properties that you obtained when registering the bot:

   ```txt
   # App Id and App Password fir the Bot Framework bot
   MICROSOFT_APP_ID=
   MICROSOFT_APP_PASSWORD=
   ```

### Register the bot in the Microsoft Teams app

The last step before you can test bot is to add it to the Microsoft Teams app manifest. You can use App Studio to do this.

1. In the browser, navigate to **https://teams.microsoft.com** and sign in with the credentials of a Work and School account.

    > **NOTE:**
    > Microsoft Teams is available for use as a web client, desktop client and a mobile client. In this module, we will use the web client but any of the clients can be used.

1. Using the app bar navigation menu, select the **More added apps** button. Then select **App Studio**:

    ![Screenshot of the More added apps dialog with App Studio](../../Linked_Image_Files/Bots/03-app-studio-02.png)

1. Select the **Manifest editor** tab and then the **Create a new app** button:

    ![Screenshot of App studio manifest editor screen - create new app](../../Linked_Image_Files/Bots/03-app-studio-03.png)

1. Provide a short name.

1. Select Generate for a new App ID.

1. Provide a Package Name and Version.

1. Provide a Short Description and Full Description.

    ![Screenshot of App studio - app details screen](../../Linked_Image_Files/Bots/03-app-studio-04.png)

1. From the **(2) Capabilities** > **Bots** page, select **Set up** to add a bot to the manifest.

1. Because you previously created a bot using the Microsoft Azure's Bot Framework, select **Existing bot** and set the following values and select **Save**:

    - **Bot ID**
      - Connect to a different bot ID: `<REPLACE_WITH_MICROSOFT_APP_ID>`
    - **Scope**: Personal, Team

1. Select **Save**.

    ![Screenshot of setting up a bot](../../Linked_Image_Files/Bots/03-app-studio-05.png)

1. Within the **Commands** section, select **Add** to add a new command to the bot.

1. On the **New command** dialog, enter the following values and select **Save**:

    - **Command text**: MentionMe
    - **Help text**: Sends message with @mention of the sender
    - **Scope**: Personal

    ![Screenshot of the new bot](../../Linked_Image_Files/Bots/03-app-studio-06.png)

1. With the bot added to the Teams app, you need to update the manifest in your project. From the **(3) Finish** > **Test and distribute** section, select the **Download** button from the **Download** section.

1. This will download the app package as a ZIP. Unpack the zip and open the **manifest.json** file in it. Copy the updated information to your project, updating the existing **./src/manifest/manifest.json** file. 

1. In the **./src/manifest/manifest.json** file, verify `icons` property's values, and update if necessary, file names to match what's in the project

1. Locate the property `bots`. Verify it contains the following JSON that includes the new command added in App Studio:

    ```json
    "bots": [
      {
        "botId": "{{MICROSOFT_APP_ID}}",
        "supportsFiles": false,
        "isNotificationOnly": false,
        "scopes": [ "team", "personal" ],
        "commandLists": [
          {
            "scopes": [ "team", "personal" ],
            "commands": [
              {
                "title": "Help",
                "description": "Shows help information"
              },
              {
                "title": "MentionMe",
                "description": "Sends message with @mention of the sender"
              }
            ]
          }
        ]
      }
    ],
    ```

    > [!IMPORTANT]
    > Notice the `botId` property value. If you see a GUID, the manifest has already been configured with the Bot App ID. If you see `{{MICROSOFT_APP_ID}}`, then this will be replaced with the value listed in the **./.env** file when you build the project.

    At this point, your bot is ready to test!

## Task 3: Test the conversational bot

1. From the command line, navigate to the root folder for the project and execute the following command:

    ```console
    gulp ngrok-serve
    ```

1. This gulp task will run many other tasks all displayed within the command-line console. The **ngrok-serve** task builds your project and starts a local web server (http://localhost:3007). It then starts ngrok with a random subdomain that creates a secure URL to your local webserver.

    > **NOTE:**
    > Microsoft Teams requires all content displayed within a tab be loaded from an HTTPS request. In development, can be done using the tool [ngrok](https://www.ngrok.com) that creates a secure rotatable URL to your local HTTP webserver. Ngrok is included as a dependency within the project so there is nothing to setup or configure.

    ![Screenshot of gulp ngrok-serve](../../Linked_Image_Files/Bots/03-test-01.png)

    Note the URL of the Ngrok URL displayed in the console. In the previous screenshot, NGrok has created the temporary URL **5f1f02998d18.ngrok.io** that will map to our locally running web server. In order for the Bot Framework to route messages from Microsoft Teams to our locally running bot, you need to update the bot's messaging endpoint in the Azure portal.

1. Open a browser and navigate to the [Azure portal](https://portal.azure.com) and sign in using a **Work or School Account** that has rights to create resources in your Azure subscription.

1. Locate the bot by selecting the Azure Resource Group and Bot Channels Registration resource you created at the beginning of this exercise.

1. Using the left-hand navigation, select **Bot management** > **Settings**.

1. Locate the property **Configuration** > **Messaging endpoint** and set the domain to the NGrok domain, pointing to the `/api/messages` endpoint. Using the example above, the endpoint will be `https://5f1f02998d18.ngrok.io/api/messages`

    ![The messaging endpoint set to https://5f1f02998d18.ngrok.io/api/messages](../../Linked_Image_Files/Bots/03-test-07.png)

1. Finally, save your changes to the bot configuration using the **Save** button at the top of the page.

    > [!IMPORTANT]
    > The free version of Ngrok will create a new URL each time you restart the web server. Make sure you update the **Messaging endpoint** of your URL each time you restart the web server when you're testing the app.

### Install the custom app in Microsoft Teams

1. Now let's install the app in Microsoft Teams. In the browser, navigate to **https://teams.microsoft.com** and sign in with the credentials of a Work and School account.

    > **NOTE:**
    > Microsoft Teams is available for use as a web client, desktop client and a mobile client. In this module, we will use the web client but any of the clients can be used.

1. Using the app bar navigation menu, select the **More added apps** button. Then select **Browse all apps** followed by **Upload for me or my teams**.

    ![Screenshot of More added apps dialog in Microsoft Teams](../../Linked_Image_Files/Bots/03-test-02.png)

1. In the file dialog that appears, select the Microsoft Teams package in your project. This app package is a ZIP file that can be found in the project's **./package** folder.

1. Once the package is uploaded, Microsoft Teams will display a summary of the app. Here you can see some "todo" items to address. _None of these "todo" items are important to this exercise, so you'll leave them as is._

    ![Screenshot of Microsoft Teams app](../../Linked_Image_Files/Bots/03-test-03.png)

1. Select the **Add** button to install the app, adding a new personal tab to your **More added apps** dialog:

    ![Screenshot of the installed Microsoft Teams app in the More added apps dialog](../../Linked_Image_Files/Bots/03-test-04.png)

1. Select the app to navigate to chat with the bot:

    ![Screenshot of the installed Microsoft Teams app](../../Linked_Image_Files/Bots/03-test-05.png)

1. Notice the commands that the bot supports are shown in the compose box when the app loads. Let's test the bot!

1. Select the **MentionMe** command, or manually type **mentionme** in the compose box, then press <kbd>enter</kbd>.

1. After a few seconds, you should see the bot respond mentioning the user you are signed in with:

    ![Screenshot of the working Microsoft Teams app](../../Linked_Image_Files/Bots/03-test-06.png)

    At this point, we have a working bot that is responding when it's mentioned.

## Summary

In this exercise, you learned how to create and add a new bot to a Microsoft Teams app and interact with it from the Microsoft Teams client.
