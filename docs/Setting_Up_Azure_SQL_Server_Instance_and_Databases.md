
# Setting Up Azure SQL Server instance and Databases

**Step 1: Setting Up Azure SQL Server instance and Databases**

1. Log in to the Azure portal (https://portal.azure.com).
2. Click on the "+ Create a resource" button.
3. Search for "SQL Server" in the search bar and select "SQL Server Single server".
4. Fill in the necessary details like Subscription, Resource Group, Server name, Server admin login, Password, and Location.
5. Select the desired Compute + storage configuration.
6. Click on "Review + create" and then "Create" to deploy the SQL Server instance.

**Step 2: Create Empty Azure SQL Server Databases in Azure**

1. Navigate to the created SQL Server instance in the Azure portal.
2. Click on "Databases" in the left-hand menu.
3. Click on the "+ Add" button to create a new database.
4. Fill in the necessary details like Database name (e.g., "SharedDb" or "HangfireDb"), Edition, Compute + storage, Collation, and click on "Review + create".
5. Finally, click on "Create" to deploy the empty Azure SQL Server database.

**Step 3: Configuring Shared Database (SharedDb)**

1. Generate script from the local SharedDb via SSMS.
2. Launch SQL Server Management Studio (SSMS) and connect to your local SQL Server instance.
3. Expand Databases, locate your SharedDb database, right-click on it, and select "Tasks" > "Generate Scripts".
4. In the Generate Scripts Wizard, select "Select specific database objects".
5. In the "Choose Objects" step, select the checkbox next to "Tables".
6. Click on the "Advanced" button.
7. In the "Advanced Scripting Options" dialog, locate the "Types of data to script" section.
8. Select "Schema Only" from the "Types of data to script" dropdown menu.
9. Click "OK" to close the "Advanced Scripting Options" dialog.
10. Proceed through the wizard, choose to script to a new query window or a file, and complete the wizard.

**Step 4: Connect to Azure SQL Server instance via SSMS and Execute the script against the Azure SharedDb via SSMS**

1. Launch SQL Server Management Studio (SSMS).
2. Connect to your Azure SQL Server instance using the server name, login, and password created earlier.
3. Open the script generated in step 3, point 10.
4. Switch the connection to the Azure SQL Server instance.
5. Once connected, ensure that the Object Explorer window displays the databases hosted on the Azure SQL Server instance.
6. Execute the script against the Azure SharedDb database to create the schema.

**Step 5: Configuring Hangfire Database (HangfireDb)**
1. Create an empty Azure SQL Database using the Azure portal.
2. Log in to the Azure portal (https://portal.azure.com).
3. Navigate to the SQL Server instance created earlier.
4. Click on "Databases" in the left-hand menu.
5. Click on the "+ Add" button to create a new database.
6. Provide the name "HangfireDb", fill in other necessary details, and click on "Review + create".
7. Finally, click on "Create" to deploy the empty Azure SQL Database for HangfireDb.
