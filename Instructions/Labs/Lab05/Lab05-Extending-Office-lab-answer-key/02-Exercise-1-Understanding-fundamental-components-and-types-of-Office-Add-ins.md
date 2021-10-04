# Exercise 1: Understanding fundamental components and types of Office Add-ins

## Task 1: Build an Excel task pane add-in

1. Install the Yeoman generator for Office Add-ins. Open PowerShell. To install globally, run the following command via the command prompt: `npm install -g yo generator-office`

    **Note**:
    Even if you've previously installed the Yeoman generator, we recommend that you update your package to the latest version from npm.
1. Run the following command to create an add-in project using the Yeoman generator: `yo office`

    **Note**:
    When you run the `yo office` command, you may receive prompts about the data collection policies of Yeoman and the Office Add-in CLI tools. Use the information that's provided to respond to the prompts as you see fit.
1. When prompted, provide the following information to create your add-in project:

    - **Choose a project type:** Office Add-in Task Pane project

    - **Choose a script type:** JavaScript

    - **What do you want to name your add-in?** My Office Add-in

    - **Which Office client application would you like to support?** Excel

    ![Welcome to the Office Add-in Generator](../../Linked_Image_Files/understanding_office_javascript_apis_image_1.png)

1. After you complete the wizard, the generator creates the project and installs supporting Node components.

    **Note**:
    You can ignore the next steps guidance that the Yeoman generator provides after the add-in project's project has been created. The step-by-step instructions within this article provide all of the guidance you'll need to complete this tutorial.
### Explore the project

The add-in project that you've created with the Yeoman generator contains sample code for a very basic task pane add-in. If you'd like to explore the components of your add-in project, open the project in your code editor and review the list of the following files. When you're ready to try out your add-in, proceed to the next section.

The **./manifest.xml** file in the root directory of the project defines the settings and capabilities of the add-in.The **./src/taskpane/taskpane.html** file contains the HTML markup for the task pane.The **./src/taskpane/taskpane.css** file contains the CSS that's applied to content in the task pane.The **./src/taskpane/taskpane.js** file contains the Office JavaScript API code that facilitates interaction between the task pane and the Office host application.### Try it out

1. Navigate to the root folder of the project.

1. Run `cd "My Office Add-in"`

1. Complete the following steps to start the local web server and sideload your add-in.

    **Note**:
    Office Add-ins should use HTTPS, not HTTP, even when you are developing. If you are prompted to install a certificate after you run one of the following commands, accept the prompt to install the certificate that the Yeoman generator provides.
    - If you're testing your add-in on a Mac, run the following command in the root directory of your project before proceeding. When you run this command, the local web server starts: `npm run dev-server`

    - To test your add-in in Excel, run the following command in the root directory of your project. This starts the local web server (if it's not already running) and opens Excel with your add-in loaded: `npm start`

    - To test your add-in in Excel for the web, run the following command in the root directory of your project. When you run this command, the local web server will start (if it's not already running): `npm run start:web`

1. To use your add-in, open a new workbook in Excel on the web and then sideload your add-in by following the instructions in [Sideload Office Add-ins in Office on the web](https://docs.microsoft.com/en-us/office/dev/add-ins/testing/sideload-office-add-ins-for-testing).

1. In Excel, choose the **Home** tab, and then choose the **Show Taskpane** button in the ribbon to open the add-in task pane.

    ![Show Taskpane button ](../../Linked_Image_Files/understanding_office_javascript_apis_image_2.png)

1. Select any range of cells in the worksheet.

1. At the bottom of the task pane, choose the **Run** link to set the color of the selected range to yellow.

![Run link](../../Linked_Image_Files/understanding_office_javascript_apis_image_3.png)

