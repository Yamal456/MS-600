# Exercise 3: Understanding customization of add-ins

## Task 1: Insert Excel charts using Microsoft Graph in a PowerPoint add-in

### Download zip file

Download the zip file from [https://github.com/OfficeDev/PowerPoint-Add-in-Microsoft-Graph-ASPNET-InsertChart/releases/download/88256/Insert_Excel_charts_using_Microsoft_Graph_in_a_PowerPoint_Add_in.zip](https://github.com/OfficeDev/PowerPoint-Add-in-Microsoft-Graph-ASPNET-InsertChart/releases/download/88256/Insert_Excel_charts_using_Microsoft_Graph_in_a_PowerPoint_Add_in.zip).

### Configure the project

1. Register your application using the [Azure Management Portal](https://manage.windowsazure.com/). Sign-in with the account of an administrator or your Office 365 subscription. Use the following settings:

    - **REDIRCT URI:** https://localhost:44301/AzureADAuth/Authorize

    - **SUPPORTED ACCOUNT TYPES:** "Accounts in this organizational directory only"

    - **IMPLICIT GRANT:** Do not enable any Implicit Grant options

    - **API PERMISSIONS:** Files.Read.All and User.Read

        **Note**:
        After you register your application, copy the Application (client) ID and the Directory (tenant) ID on the Overview blade of the App Registration in the Azure Management Portal. When you create the client secret on the Certificates and secrets blade, copy it too.
1. In `web.config`, use the values that you copied in the previous step. Set `AAD:ClientID` to your client ID, set `AAD:ClientSecret` to your client secret, and set `"AAD:O365TenantID"` to your tenant ID.

1. Open the solution in Visual Studio, and open Solution Explorer and then choose the **PowerPoint-Add-in-Microsoft-Graph-ASPNET-InsertChartWeb** project. In **Properties**, ensure **SSL Enabled** is **True**. Verify that the **SSL URLproperty** uses the same domain name and port number as those listed previously.

1. In Solution Explorer, right-click the topmost node—the **Solution ...** node. Select **Set Startup Projects**. In the dialog that opens, expand **Common Properties** and select **Startup Project**. Enable Multiple startup projects. Ensure that the project whose name ends with "Web" is listed first and that both projects are set to **Start** in the **Action** column.

### Run the project

1. Build the solution.

1. Press F5.

1. In PowerPoint, open the Insert tab, and select **Pick a chart to open the task pane add-in**. The home page provides instructions.

### Known issue 1: When trying to run the code sample, the add-in will not load.

**Resolution:**

1. In Visual Studio, open SQL Server Object Explorer.

1. Expand **(localdb)\MSSQLLocalDB** > **Databases**.

1. Right-click **PowerPoint-Add-in-Microsoft-Graph-ASPNET-InsertChart**, then choose **Delete**.

### Known issue 2: When you run the code sample, you get an error on the line Office.context.ui.messageParent.

**Resolution:** Stop running the code sample and restart it.

### Known issue 3: When you download a zip file and extract the files, you get an error indicating that the file path is too long.

**Resolution:** Unzip your files to a folder directly under the root (e.g. **c:\sample**).

