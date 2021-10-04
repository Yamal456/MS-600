# Exercise 6: Create a custom Microsoft Teams personal tab

In this exercise, you'll create a new Microsoft Teams personal tab by using the Microsoft Teams Yeoman generator, Visual Studio Code, and App Studio.

## Task 1: Create Microsoft Teams app

1. Open your command prompt, and go to a directory where you want to save your work. Create a new folder named **learn-msteams-tabs**, and change the directory into that folder.

1. Run the Yeoman generator for Microsoft Teams by running the following command:

    ```console
    yo teams
    ```

    ![Screenshot of the Yeoman generator for Microsoft Teams](../../Linked_Image_Files/Tabs/03-yo-teams-01.png)

1. Yeoman starts and asks you a series of questions. Answer the questions with the following values:

    - **What is your solution name?**: Learn MSTeams Tabs
    - **Where do you want to place the files?**: Use the current folder
    - **Title of your Microsoft Teams App project**: Learn MSTeams Tabs
    - **Your (company) name (max 32 characters)**: Contoso
    - **Which manifest version would you like to use?**: v1.8
    - **Quick scaffolding**: Yes
    - **What features do you want to add to your project?**: A tab
    - **The URL where you will host this solution?**: (Accept the default option)
    - **Would you like to show a loading indicator when your app/tab loads?** No
    - **Default Tab name (max 16 characters)**: LearnPersonalTab
    - **What kind of Tab would you like to create?**: Personal (static)
    - **Do you require Azure AD Single-Sign-On support for the tab?** No

    > **NOTE:**
    > Most of the answers to these questions can be changed after you create the project. For example, the URL where the project will be hosted isn't important at the time of creating or testing the project.

After you answer the generator's questions, the generator creates the scaffolding for the project. The generator then runs `npm install` that downloads all the dependencies required by the project.

## Task 2: Test the personal tab

Before you customize the tab, let's test the tab to see the initial developer experience for testing.

1. From the command line, go to the root folder for the project and run the following command:

    ```console
    gulp ngrok-serve
    ```

    This gulp task runs many other tasks all displayed within the command-line console. The ngrok-serve task builds your project and starts a local web server (http://localhost:3007). It then starts ngrok with a random subdomain that creates a secure URL to your local web server.

    > **NOTE:**
    > Microsoft Teams requires all content displayed within a tab to be loaded from an HTTPS request. In development, this can be done by using the tool [ngrok](https://www.ngrok.com) that creates a secure rotatable URL to your local HTTP web server. Ngrok is included as a dependency within the project, so there's nothing to set up or configure.

    ![Screenshot of gulp ngrok-serve](../../Linked_Image_Files/Tabs/03-yo-teams-02.png)

1. Open a browser, and go to the ngrok URL displayed in the console.

    ![Screenshot of the local web app hosting the Teams tab project - project homepage](../../Linked_Image_Files/Tabs/03-yo-teams-03.png)

1. Update the URL in the browser to load the tab created by the scaffolding process. Here you can see the page can determine that it isn't running within the Microsoft Teams client.

    ![Screenshot of the local web app hosting the Teams tab project - displaying the tab](../../Linked_Image_Files/Tabs/03-yo-teams-04.png)

1. Now let's load the tab in Microsoft Teams. In the browser, go to [Microsoft Teams](https://teams.microsoft.com). Sign in with the credentials of a Work and School account.

    > **NOTE:**
    > Microsoft Teams is available for use as a web client, a desktop client, and a mobile client. In this module, we use the web client, but any of the clients can be used.

1. In the app bar on the left, select the **More added apps** button. Then select **More apps**.

    ![Screenshot of More added apps dialog box in Microsoft Teams](../../Linked_Image_Files/Tabs/03-yo-teams-05.png)

1. On the **Get more done with apps!** page, select **Upload a custom app** > **Upload for me or my teams**.

    ![Screenshot of available apps and services page in Microsoft Teams](../../Linked_Image_Files/Tabs/03-yo-teams-05a.png)

1. In the file dialog box that appears, select the Microsoft Teams package in your project. This app package is a zip file in the project's **./package** folder.

1. After the package is uploaded, select it to display a summary of the app. Here you can see some todo items to address. You'll update the todo items later in the exercise.

    ![Screenshot of Microsoft Teams app](../../Linked_Image_Files/Tabs/03-yo-teams-06.png)

1. Select the **Add** button to install the app, which adds a new personal tab to your **More added apps** dialog box.

    ![Screenshot of the installed Microsoft Teams app in the More added apps dialog box](../../Linked_Image_Files/Tabs/03-yo-teams-07.png)

1. Select the app to go to the new tab.

    ![Screenshot of the installed Microsoft Teams app personal tab](../../Linked_Image_Files/Tabs/03-yo-teams-08.png)

    Notice that when the content page is loaded in a tab within the Microsoft Teams client, it displays the `entityId` property of the tab, not the message "This isn't hosted in Microsoft Teams" as you saw when you viewed the content page in the browser. The tab can detect if it's loaded within the Microsoft Teams client by using the Microsoft Teams JavaScript SDK.

    The next step is to make some changes to the project.

1. Uninstall the app by right-clicking the app in the **More added apps** dialog box and select **Uninstall**. Then select **Uninstall** in the confirmation dialog box that appears.

    ![Screenshot uninstalling the Microsoft Teams app](../../Linked_Image_Files/Tabs/03-yo-teams-09.png)

    ![Screenshot of the Uninstall dialog box for the Microsoft Teams app](../../Linked_Image_Files/Tabs/03-yo-teams-10.png)

1. Next, stop the local web server by selecting <kbd>Ctrl</kbd>+<kbd>C</kbd> in the console to stop the running process.

## Task 3: Implement the personal tab's user interface

Now you can implement the user interface for the tab. The simple tab has a basic interface. It presents a list of items, and users can add items to the list.

1. Locate and open the file that contains the React component used in the project: **./src/client/learnPersonalTab/LearnPersonalTab.tsx**.

1. Update the `import` statements in this file to add components from the Fluent UI - React library. Find the following `import` statement at the top of the file that imports components from the Fluent UI - React library:

    ```typescript
    import { Provider, Flex, Text, Button, Header } from "@fluentui/react-northstar";
    ```

1. Replace the previous statement with the following `import` statement:

    ```typescript
    import { Provider, Flex, Text, Button, Header,
      List, Alert, WindowMaximizeIcon, ExclamationTriangleIcon,
      Label, Input, ToDoListIcon
    } from "@fluentui/react-northstar";
    ```

1. Next, update the state of the component to contain a list of to do items and a property for a new item. Add the following statements after the existing `useState` statements in the **LearnPersonalTab.tsx** file:

    ```typescript
    const [todoItems, setTodoItems] = useState<string[]>(["Submit time sheet","Submit expense report"]);
    const [newTodoValue, setNewTodoValue] = useState<string>("");
    ```

1. Locate the `return` statement, and update it to the following code. The component now displays the list of items in our state with a brief copyright statement.

    ```tsx
    return (
      <Provider theme={theme}>
        <Flex column gap="gap.smaller">
          <Header content="This is your tab" />
          <Alert icon={<ExclamationTriangleIcon />} content={entityId} dismissible></Alert>
          <Text content="These are your to-do items:" size="medium"></Text>
          <List selectable>
            {todoItems.map((todoItem, i) => (
              <List.Item media={<WindowMaximizeIcon outline />}
                content={todoItem} index={i} >
              </List.Item>))
            }
          </List>

          TODO: add new list item form here

          <Text content="(C) Copyright Contoso" size="smallest"></Text>
        </Flex>
      </Provider>
    );
    ```

    > **TIP:**
    > At this point, you can test your tab without loading it in Microsoft Teams. Run the command **gulp ngrok-serve** from the command line, and go to **https://`{your-ngrok-subdomain}`.ngrok.io/learnPersonalTab/index.html** in the browser.
    >
    > ![Screenshot of the update tab page in the default theme](../../Linked_Image_Files/Tabs/03-yo-teams-11.png)
    >
    > Add the query string value `?theme=dark` to the URL to see the theme change:
    >
    > ![Screenshot of the update tab page in the dark theme](../../Linked_Image_Files/Tabs/03-yo-teams-12.png)

1. The next step is to add some interactivity to the tab. Add the following methods to the `LearnPersonalTab` class. These methods handle updating the state when specific events happen on the form that you'll add to the component.

    ```typescript
    const handleOnChanged = (event): void => {
      setNewTodoValue(event.target.value);
    }

    const handleOnClick = (event: React.MouseEvent<HTMLButtonElement>): void => {
      const newTodoItems = todoItems;
      newTodoItems.push(newTodoValue);
      setTodoItems(newTodoItems);
      setNewTodoValue("");
    }
    ```

1. Finally, locate the string `TODO: add new list item form here` in the `render()` method, and replace it with the following code. This code displays a form that the user can use to add items to the list.

    ```tsx
    <Flex gap="gap.medium">
      <Flex.Item grow>
        <Flex>
          <Label icon={<ToDoListIcon />}
                styles={{
                  background: "darkgray",
                  height: "auto",
                  padding: "0 15px"
                }}></Label>
          <Flex.Item grow>
            <Input placeholder="New todo item" fluid
                  value={newTodoValue}
                  onChange={handleOnChanged}></Input>
          </Flex.Item>
        </Flex>
      </Flex.Item>
      <Button content="Add Todo" primary
              onClick={handleOnClick}></Button>
    </Flex>
    ```

## Task 4: Use App Studio to update the Microsoft Teams app manifest

At this point, the app is complete. Recall from our initial test that when the app was added to Microsoft Teams, it had a few todo strings for the description of the app. While you could change these values in the project's **./src/manifest/manifest.json** file, you'll use App Studio to make these changes.

1. First, build, and run the project by running the command **gulp ngrok-serve** in the command line like you did previously. This step also creates the Microsoft Teams app package.

1. In the browser, go to [Microsoft Teams](https://teams.microsoft.com) and sign in with the credentials of a Work and School account.

1. Using the **More added apps** link in the app bar on the left, select **App Studio**.

    ![Screenshot of the More added apps dialog box with App Studio highlighted](../../Linked_Image_Files/Tabs/03-yo-teams-13.png)

1. Select the **Manifest editor** tab in App Studio, and then select **Import an existing app**. Locate the zip file that can be found in the project's **./package** folder and open it.

    ![Screenshot of the Microsoft Teams app in App Studio](../../Linked_Image_Files/Tabs/03-yo-teams-14.png)

1. Edit the app by selecting its tile, or use the menu in the upper-right corner for more options and select **Edit**.

1. On the **App details** page, change the **Full name** of the app to **Learn Teams Tabs**.

1. On the **App details** page, change the **Version** to **1.0.0**.

1. On the **App details** page, scroll down to the **Descriptions** section and enter the following values:

    - **Short description**: My first custom Teams app
    - **Full description**: *enter a long description*

    ![Screenshot of app details App Studio](../../Linked_Image_Files/Tabs/03-yo-teams-15.png)

1. Update the name of the tab by selecting **Capabilities** > **Tabs** in the left pane in App Studio.

1. Locate the only personal tab in the project. Select the menu for more options on the tab, and select **Edit**. Change the name of the tab to **My First Tab**. Update the **Content URL** property with your new ngrok subdomain. Ensure that `&theme={theme}` is added to the end of the Content URL property. Select **Save** to save your changes.

1. The changes made to the app within App Studio aren't saved to your project. If you want to update the project, download the app package from App Studio.

    To download the project, select **Finish** > **Test and distribute** in the left pane in App Studio. Then select **Download**.

    > **CAUTION:**
    > Be careful if you chose to update the **manifest.json** file in your project with the one in the package downloaded from App Studio.
    >
    > The manifest file in your project contains placeholder strings that are updated by the build and debugging process that's replaced when you test the project. Using placeholder strings simplifies the development and debugging process.
    >
    > For example, the placeholder `{{HOSTNAME}}` is replaced with the hosting URL of the app each time the package is re-created.
    >
    > So you might not want to completely replace the existing **manifest.json** file with the file generated by App Studio.

## Task 5: Install and test the Microsoft Teams app

1. In App Studio, select **Finish** > **Test and distribute** in the left pane in App Studio. Then select **Install**. Notice that the new names and descriptions are shown before installing the app.

    ![Screenshot of installing the updated app](../../Linked_Image_Files/Tabs/03-yo-teams-16.png)

1. Select **Add** to install the app. This action adds a new personal tab to your **More added apps** dialog box. Select the app to see the updated working version.

    ![Screenshot of the updated and working tab](../../Linked_Image_Files/Tabs/03-yo-teams-17.png)

## Summary

In this exercise, you created a new Microsoft Teams personal tab by using the Microsoft Teams Yeoman generator, Visual Studio Code, and App Studio.
