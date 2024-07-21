
# Deploying .NET Web API Project and Blazor App Project to IIS Server

This guide outlines the steps to deploy a .NET Web API project and a Blazor App project to an Internet Information Services (IIS) Server. By following these steps, you'll be able to make your application accessible through the web.

## Prerequisites

1. **IIS Server**: Ensure that IIS is installed on your server.
2. **.NET SDK**: Install .NET SDK on your development machine.
3. **Publish Profiles**: Create publish profiles for both the Web API project and the Blazor App project.

### Step 1: Publish Web API Project

1. Open the Web API project solution in Visual Studio.

2. Right-click on the Web API project and select "Publish".

3. Choose "Folder" as the publish target.

4. Configure the publish profile settings as per your requirements. Ensure that the target framework is set appropriately.

5. Click on "Publish" to generate the published files.

6. Once the publishing process is complete, navigate to the published folder and ensure all necessary files are present.

### Step 2: Publish Blazor App Project

1. Open the Blazor App project solution in Visual Studio.

2. Right-click on the Blazor App project and select "Publish".

3. Choose "Folder" as the publish target.

4. Configure the publish profile settings, ensuring the target framework matches your deployment environment.

5. Click on "Publish" to generate the published files.

6. Once the publishing process is complete, navigate to the published folder and ensure all necessary files are present.

### Step 3: Configure IIS

1. Log in to your server where IIS is installed.

2. Open Internet Information Services (IIS) Manager.

3. Create a new website for your Web API project:
   - Right-click on "Sites" and select "Add Website".
   - Enter the site name and choose the physical path where your Web API project files are located.
   - Set the binding information (e.g., IP address, port).

4. Repeat the above steps to create another website for your Blazor App project.

### Step 4: Deploy Web API Project

1. Copy the published files of the Web API project to the physical path of the corresponding IIS website.

2. Ensure that necessary permissions are set on the directory to allow the application pool identity to access the files.

3. Test the Web API by navigating to its URL in a web browser.

### Step 5: Deploy Blazor App Project

1. Copy the published files of the Blazor App project to the physical path of the corresponding IIS website.

2. Ensure that necessary permissions are set on the directory to allow the application pool identity to access the files.

3. Test the Blazor App by navigating to its URL in a web browser.

### Step 6: Final Testing

1. Test the integration between the Web API and the Blazor App to ensure they communicate properly.

2. Perform thorough testing of your applications to ensure they function correctly in the production environment.

## Conclusion

By following these steps, you can successfully deploy your .NET Web API project and Blazor App project to an IIS Server, making them accessible over the web. Remember to regularly update and maintain your deployed applications for optimal performance and security.
