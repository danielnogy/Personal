
# Deploying BinaryPlate.Gateway to Azure

**Step 1: Open the BinaryPlate.Gateway Solution in Visual Studio 2022**

1.  Launch Visual Studio 2022.
2.  Open the BinaryPlate.Gateway project solution.


**Step 2: Adjust URLs in appsettings.json**

1.  Locate the **appsettings.json** file in your project's wwwroot directory.
2.  Find the settings for **BaseApiUrl**,  and **BaseTenantClientUrl**.
3.  Update the **BaseApiUrl** setting with the extracted WebAPI URL obtained in the previous step.

Example of the updated **appsettings.json**:
```
{
  // Example: "BaseApiUrl": https://binaryplatewebapi.azurewebsites.net/api/
  "BaseApiUrl": "https://path.to.web.api.app/api/",
  // Example: "BaseClientUrl": https://{0}.myblazorapp.azurewebsites.net/
  "BaseTenantClientUrl": "https://{0}.path.to.blazor.app.service/"
}
```

**Step 3: Build and Test Locally**

1.  Build the BinaryPlate.Gateway project in Visual Studio to ensure that there are no build errors.
2.  Run the project locally to verify that the application loads and connects to the BinaryPlate.WebAPI project without any issues. Perform necessary tests to ensure functionality.

**Step 5: Publish the Project to Azure**

1.  Right-click on the BinaryPlate.Gateway project in Solution Explorer.
2.  Select "Publish" to open the publish dialog.
3.  Choose "Azure" as the target for publishing.
4.  Sign in to your Azure account if prompted.
5.  Select or create a new Azure App Service where you want to deploy the project.
6.  Configure any additional settings such as deployment slots, application insights, etc., as needed.
7.  Click "Publish" to initiate the deployment process.

**Step 6: Monitor Deployment Progress**

1.  Visual Studio will display the progress of the deployment.
2.  Once the deployment is complete, you can view the deployed application in your Azure App Service.

**Step 7: Verify Deployment**

1.  Access the deployed BinaryPlate.Gateway application using the URL of your Azure App Service.
2.  Perform thorough testing to ensure that the application behaves as expected in the Azure environment.

By following these steps, you can successfully deploy the BinaryPlate.Gateway project to Azure using Visual Studio 2022 and ensure that it connects to your BinaryPlate.WebAPI project hosted in Azure. Adjusting the URLs in the **appsettings.json** file ensures that your application seamlessly integrates with the Azure resources.
