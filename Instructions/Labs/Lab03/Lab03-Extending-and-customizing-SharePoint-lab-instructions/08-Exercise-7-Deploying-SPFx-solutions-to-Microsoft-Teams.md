# Exercise 7: Deploying SPFx solutions to Microsoft Teams

## Task 1: Create your project

1. From the PowerShell command prompt, change to the C:/LabFiles/SharePoint directory by executing the following command: `cd c:/LabFiles/SharePoint`

1. Make a new directory for your SharePoint project files by executing the following command: `md SPFxTeamsTab`

1. Navigate to the newly created SharePoint directory by executing the following command: `cd SPFxTeamsTab`

1. Run the SharePoint Yeoman generator by executing the following command: `yo @microsoft/sharepoint`

1. Use the following to complete the prompt that is displayed:

    - **What is your solution name?**: SPFxTeamsTab

    - **Which baseline packages do you want to target for your component(s)?:** SharePoint Online only (latest)

    - **Where do you want to place the files?**: Use the current folder

    - **Do you want to allow the tenant admin the choice of being able to deploy the solution to all sites immediately without running any feature deployment or adding apps in sites?**: Yes

    - **Will the components in the solution require permissions to access web APIs that are unique and not shared with other components in the tenant?**: No

    - **Which type of client-side component to create?**: WebPart

    - **What is your Web Part name?**: SPFx Teams Together

    - **What is your Web Part description?**: SPFx Teams Together description

    - **Which framework would you like to use?**: No JavaScript framework

1. After provisioning the folders required for the project, the generator will install all the dependency packages using NPM.

1. Open the project in an editor such as Visual Studio Code.

1. Enable the web part to be used in Microsoft Teams:

    1. Locate and open the file **./src/webparts/spFxTeamsTogether/SpFxTeamsTogetherWebPart.manifest.json**.

    1. Within the web part manifest file, locate the property **supportedHosts**: `"supportedHosts": ["SharePointWebPart"],`

    1. Add another option to enable this web part to be used as a tab in a Microsoft Teams team: `"supportedHosts": ["SharePointWebPart", "TeamsTab"],`

## Task 2: Creating and deploying the Microsoft Teams app package

Notice the project contains a folder **teams** that contains two images. These are used in Microsoft Teams to display the custom tab.

You may notice there is no **manifest.json** file present. The manifest file can be generated automatically by SharePoint from the App Catalog site. However, at this time, this functionality is not fully operational.

Alternatively, you can manually create the manifest file if you want to have more control over the default values that SharePoint will create.

The automatic generation and deployment of the Microsoft Teams manifest is not currently operational; you will manually create the Microsoft Teams manifest and app package:

1. Create a **manifest.json** file in **./teams** folder, use the following manifest file as a template:
    ```json
    {
        "$schema": "https://developer.microsoft.com/json-schemas/teams/v1.5/MicrosoftTeams.schema.json",
        "manifestVersion": "1.5",
        "packageName": "{{SPFX_COMPONENT_ALIAS}}",
        "id": "{{SPFX_COMPONENT_ID}}",
        "version": "0.1",
        "developer": {
            "name": "Parker Porcupine",
            "websiteUrl": "https://contoso.com",
            "privacyUrl": "https://contoso.com/privacystatement",
            "termsOfUseUrl": "https://contoso.com/servicesagreement"
        },
        "name": {
            "short": "{{SPFX_COMPONENT_NAME}}"
        },
        "description": {
            "short": "{{SPFX_COMPONENT_SHORT_DESCRIPTION}}",
            "full": "{{SPFX_COMPONENT_LONG_DESCRIPTION}}"
        },
        "icons": {
            "outline": "{{SPFX_COMPONENT_ID}}_outline.png",
            "color": "{{SPFX_COMPONENT_ID}}_color.png"
        },
        "accentColor": "#004578",
        "staticTabs": [
            {
            "entityId": "com.contoso.personaltab.spfx",
            "name": "My SPFx Personal Tab",
            "contentUrl": "https://{teamSiteDomain}/_layouts/15/TeamsLogon.aspx?SPFX=true&dest=/_layouts/15/teamshostedapp.aspx%3Fteams%26personal%26componentId={{SPFX_COMPONENT_ID}}%26forceLocale={locale}",
            "scopes": [
                "personal"
            ]
            }
        ],
        "configurableTabs": [
            {
            "configurationUrl": "https://{teamSiteDomain}{teamSitePath}/_layouts/15/TeamsLogon.aspx?SPFX=true&dest={teamSitePath}/_layouts/15/teamshostedapp.aspx%3FopenPropertyPane=true%26teams%26componentId={{SPFX_COMPONENT_ID}}%26forceLocale={locale}",
            "canUpdateConfiguration": true,
            "scopes": [
                "team"
            ]
            }
        ],
        "validDomains": [
            "*.login.microsoftonline.com",
            "*.sharepoint.com",
            "spoprod-a.akamaihd.net",
            "resourceseng.blob.core.windows.net"
        ],
        "webApplicationInfo": {
            "resource": "https://{teamSiteDomain}",
            "id": "00000003-0000-0ff1-ce00-000000000000"
        }
    }
    ```

1. Open the **manifest.json** file. This file contains multiple strings that need to be updated to match the SPFx component. Use the following table to determine the values that should be replaced. The SPFx component properties are found in the web part manifest file: **./src/webparts/spFxTeamsTogether/SpFxTeamsTogetherWebPart.manifest.json**

    | **manifest.json string**| **Property in SPFx component manifest**|
    | :--- | :--- |
    | {{SPFX_COMPONENT_ALIAS}}| alias|
    | {{SPFX_COMPONENT_NAME}}| preconfiguredEntries[0].title|
    | {{SPFX_COMPONENT_SHORT_DESCRIPTION}}| preconfiguredEntries[0].description|
    | {{SPFX_COMPONENT_LONG_DESCRIPTION}}| preconfiguredEntries[0].description|
    | {{SPFX_COMPONENT_ID}}| id|

    **Note**:
    Don't miss replacing **{{SPFX_COMPONENT_ID}}** in **configurableTabs[0].configurationUrl**. You will likely have to scroll your editor to the right to see it. The tokens surrounded by single curly braces (for example, **{teamSiteDomain}**) do not need to be replaced.

1. Create a Microsoft Teams app package by zipping the contents of the **./teams** folder. Make sure to zip just the contents and not the folder itself. This ZIP archive should contain three files at the root: two images and the **manifest.json**.

## Task 3: Create and deploy the SharePoint package

In order to test the web part in SharePoint and Microsoft Teams, it must first be deployed to an app catalog.

1. Open a browser and navigate to your SharePoint Online Tenant-Scoped App Catalog site.

1. Select the menu item **Apps for SharePoint** from the left navigation menu.

1. Build the project by opening a command prompt and changing to the root folder of the project. Then execute the following command: `gulp build`

1. Next, create a production bundle of the project by running the following command on the command line from the root of the project: `gulp bundle --ship`

1. Create a deployment package of the project by running the following command on the command line from the root of the project: `gulp package-solution --ship`

1. Locate the file created by the gulp task, found in the **./sharepoint/solution** folder with the name *.sppkg.

1. Drag this file into **the Apps for SharePoint** library in the browser.

1. In the **Do you trust...?** dialog box, select the check box **Make this solution available to all sites in the organization** and then select **Deploy**.

1. This will make the SPFx web part available to all site collections in the tenant, including those that are behind a Microsoft Teams team.

## Task 4: Test the web part in SharePoint and Microsoft Teams

Test the SPFx web part in SharePoint:

1. In the browser, navigate to a modern SharePoint page.

1. Select the **Edit** button in the upper-right portion of the page.

1. Select the **(+)** icon to open the SharePoint web part toolbox and locate the web part SPFx Teams Together:

1. The SharePoint Framework web part will be displayed on the page.

## Task 5: Testing the SPFx web part in Microsoft Teams

1. Create a new Microsoft Teams team.

    1. Using the same browser where you are signed in to SharePoint Online, navigate to [https://teams.microsoft.com](https://teams.microsoft.com/). When prompted, load the web client.

    1. If you do not have any teams in your tenant, you will be presented with the following dialog. Otherwise, select **Join or create a team** at the bottom of the list of teams.

    1. On the **Create your team** dialog box, select **Build a team from scratch**.

    1. On the **What kind of team will this be?** dialog box, select **Public**.

    1. When prompted, use the name **My First Team**.

1. Install the Microsoft Teams application as a new tab that will expose the SharePoint Framework web part in Microsoft Teams:

    1. Select the **My First Team** team previously created.

    1. Select the **General** channel.

1. Add a custom tab to the team using the SPFx web part:

    1. At the top of the page, select the **+** icon in the horizontal navigation.

    1. In the **Add a tab** dialog box, select **More Apps**.

    1. Select the **Upload a custom app** > **Upload for ...** from the list of app categories.

    1. Select the Microsoft Teams application ZIP file previously created. This is the file that contains the **manifest.json** and two image files.

        **Note**:
        After a moment, the application will appear next to your tenant name. You may need to refresh the page for the app to appear if you are using the browser Microsoft Teams client.

1. Select the **SPFx Teams Together** app.

1. In the **SPFx Teams Together** dialog box, select the **My First Team** in the **Add to a team** drop-down control and select **Install**.

1. In the **SPFx Teams Together is now available for My First Team** dialog box, select the **General** channel and select **Set up**.

1. The next dialog box will confirm the installation of the app. Select **Save**.

1. The application should now load in Microsoft Teams within the **General** channel under the tab **SPFx Teams Together**.

## Review

In this exercise, you learned how to create a SharePoint Framework (SPFx) solution that will work in both SharePoint and as a tab in Microsoft Teams.

