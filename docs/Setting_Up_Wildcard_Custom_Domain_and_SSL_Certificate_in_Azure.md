
# Setting Up Wildcard Custom Domain and SSL Certificate in Azure

**Step 1: Prepare your Azure App Service**

1.  Ensure that the BinaryPlate.BlazorPlate app service is set up and running in your Azure subscription.

# Setting Up Wildcard Custom Domain and SSL Certificate in Azure

**Step 1: Prepare your Azure App Service**

1.  Ensure that the BinaryPlate.BlazorPlate app service is set up and running in your Azure subscription.

**Step 2: Configure Custom Domain in Azure**

1.  In the Azure Portal, go to the "Custom domains" section of your BinaryPlate.BlazorPlate app service.
2.  Click on "Add custom domain".
3.  Enter your wildcard custom domain name (e.g., ***.example.com**) and click "Validate".
4.  Azure will provide you with a TXT record to add to your DNS records for verification. Note down this TXT record.

**Step 3: Add DNS Records in Domain Registrar's DNS Management Interface**

1.  Log in to your domain registrar's website and navigate to the DNS management interface for your domain.
2.  Add a new TXT record with the details provided by Azure during the custom domain setup. This TXT record is used for verification purposes.
3.  Save the changes to your DNS records.

**Step 4: Verify DNS Records in Azure**

1.  Back in the Azure Portal, navigate back to the "Custom domains" section of your BinaryPlate.BlazorPlate app service.
2.  Click on the custom domain you added.
3.  Click on "Validate" to verify that the DNS records have been correctly configured.
4.  Azure will perform DNS verification, and if successful, it will validate the custom domain.

**Step 5: Assign Custom Domain to App Service**

1.  After the custom domain has been successfully validated, go back to the "Custom domains" section of your BinaryPlate.BlazorPlate app service.
2.  Click on "Add custom domain" again.
3.  Enter your wildcard custom domain name (e.g., ***.example.com**) and click "Add custom domain".

**Step 6: Configure DNS for Subdomains (Optional)**

1.  If you want to configure DNS for subdomains of your wildcard custom domain, repeat Steps 3 and 4 for each subdomain.
2.  Add the appropriate DNS records (e.g., A records, CNAME records) to point each subdomain to your Azure App Service's IP address.

By following these steps, you can successfully set up a wildcard custom domain for your BinaryPlate.BlazorPlate app service in Azure. Ensure that you verify DNS records both in Azure and in your domain registrar's DNS management interface to ensure proper configuration and validation.

  

  
Configuring a wildcard SSL certificate for your wildcard custom domain in Azure Portal involves several steps. Below is a detailed walkthrough:

**Step 1: Purchase or Obtain a Wildcard SSL Certificate**

1.  Navigate to the Azure Portal.
2.  Go to the Azure App Service where your BinaryPlate.BlazorPlate application is hosted.
3.  In the "Custom domains" section, click on the custom domain you want to configure SSL for.
4.  Under the "TLS/SSL settings" tab, click on "Private Key Certificates (.pfx)".
5.  Select "Purchase App Service Certificate" to purchase an SSL certificate directly from Azure.
6.  Choose the appropriate plan (e.g., Standard or Wildcard SSL) and pricing tier.
7.  Enter the necessary details for purchasing the SSL certificate, such as the domain name and validity period.
8.  Complete the purchase process and wait for the SSL certificate to be provisioned.

**Step 2: Configure Custom Domain SSL Binding in Azure App Service**

1.  Go back to the Azure Portal.
2.  Navigate to the App Service where your BinaryPlate.BlazorPlate application is hosted.
3.  Go to the "Custom domains" section of the App Service.
4.  Click on the custom domain you want to configure SSL for.
5.  Under the "TLS/SSL settings" tab, click on "Private Key Certificates (.pfx)".
6.  Select "Upload Certificate" and provide the necessary details:

-   Certificate file (.pfx)
-   Certificate password

7.  Click on "Add Binding" and select the custom domain you want to bind the SSL certificate to.
8.  Save the changes.

**Step 3: Enable HTTPS Only**

1.  In the "TLS/SSL settings" tab of your App Service, enable the "HTTPS Only" toggle to enforce HTTPS for all requests.

**Step 4: Verify SSL Configuration**

1.  After configuring the SSL certificate, navigate to your BinaryPlate.BlazorPlate application using the custom domain URL (e.g., **https://yourwildcarddomain.example.com**).
2.  Ensure that the browser shows a green padlock icon indicating a secure connection.
3.  Verify that the SSL certificate is issued to your wildcard custom domain.

**Step 5: Test HTTPS Functionality**

1.  Access various pages and features of your BinaryPlate.BlazorPlate application using the custom domain over HTTPS.
2.  Verify that all resources are loaded securely without any mixed content warnings or errors.

By following these steps, you can successfully set up a wildcard SSL certificate for your wildcard custom domain in Azure Portal, whether by purchasing directly from Azure or by using an SSL certificate obtained from a third-party CA. This ensures secure communication between your BinaryPlate.BlazorPlate application and its users, enhancing privacy and data integrity.
**Step 2: Configure Custom Domain in Azure**

1.  In the Azure Portal, go to the "Custom domains" section of your BinaryPlate.BlazorPlate app service.
2.  Click on "Add custom domain".
3.  Enter your wildcard custom domain name (e.g., ***.example.com**) and click "Validate".
4.  Azure will provide you with a TXT record to add to your DNS records for verification. Note down this TXT record.

**Step 3: Add DNS Records in Domain Registrar's DNS Management Interface**

1.  Log in to your domain registrar's website and navigate to the DNS management interface for your domain.
2.  Add a new TXT record with the details provided by Azure during the custom domain setup. This TXT record is used for verification purposes.
3.  Save the changes to your DNS records.

**Step 4: Verify DNS Records in Azure**

1.  Back in the Azure Portal, navigate back to the "Custom domains" section of your BinaryPlate.BlazorPlate app service.
2.  Click on the custom domain you added.
3.  Click on "Validate" to verify that the DNS records have been correctly configured.
4.  Azure will perform DNS verification, and if successful, it will validate the custom domain.

**Step 5: Assign Custom Domain to App Service**

1.  After the custom domain has been successfully validated, go back to the "Custom domains" section of your BinaryPlate.BlazorPlate app service.
2.  Click on "Add custom domain" again.
3.  Enter your wildcard custom domain name (e.g., ***.example.com**) and click "Add custom domain".

**Step 6: Configure DNS for Subdomains (Optional)**

1.  If you want to configure DNS for subdomains of your wildcard custom domain, repeat Steps 3 and 4 for each subdomain.
2.  Add the appropriate DNS records (e.g., A records, CNAME records) to point each subdomain to your Azure App Service's IP address.

By following these steps, you can successfully set up a wildcard custom domain for your BinaryPlate.BlazorPlate app service in Azure. Ensure that you verify DNS records both in Azure and in your domain registrar's DNS management interface to ensure proper configuration and validation.

  

  
Configuring a wildcard SSL certificate for your wildcard custom domain in Azure Portal involves several steps. Below is a detailed walkthrough:

**Step 1: Purchase or Obtain a Wildcard SSL Certificate**

1.  Navigate to the Azure Portal.
2.  Go to the Azure App Service where your BinaryPlate.BlazorPlate application is hosted.
3.  In the "Custom domains" section, click on the custom domain you want to configure SSL for.
4.  Under the "TLS/SSL settings" tab, click on "Private Key Certificates (.pfx)".
5.  Select "Purchase App Service Certificate" to purchase an SSL certificate directly from Azure.
6.  Choose the appropriate plan (e.g., Standard or Wildcard SSL) and pricing tier.
7.  Enter the necessary details for purchasing the SSL certificate, such as the domain name and validity period.
8.  Complete the purchase process and wait for the SSL certificate to be provisioned.

**Step 2: Configure Custom Domain SSL Binding in Azure App Service**

1.  Go back to the Azure Portal.
2.  Navigate to the App Service where your BinaryPlate.BlazorPlate application is hosted.
3.  Go to the "Custom domains" section of the App Service.
4.  Click on the custom domain you want to configure SSL for.
5.  Under the "TLS/SSL settings" tab, click on "Private Key Certificates (.pfx)".
6.  Select "Upload Certificate" and provide the necessary details:

-   Certificate file (.pfx)
-   Certificate password

7.  Click on "Add Binding" and select the custom domain you want to bind the SSL certificate to.
8.  Save the changes.

**Step 3: Enable HTTPS Only**

1.  In the "TLS/SSL settings" tab of your App Service, enable the "HTTPS Only" toggle to enforce HTTPS for all requests.

**Step 4: Verify SSL Configuration**

1.  After configuring the SSL certificate, navigate to your BinaryPlate.BlazorPlate application using the custom domain URL (e.g., **https://yourwildcarddomain.example.com**).
2.  Ensure that the browser shows a green padlock icon indicating a secure connection.
3.  Verify that the SSL certificate is issued to your wildcard custom domain.

**Step 5: Test HTTPS Functionality**

1.  Access various pages and features of your BinaryPlate.BlazorPlate application using the custom domain over HTTPS.
2.  Verify that all resources are loaded securely without any mixed content warnings or errors.

By following these steps, you can successfully set up a wildcard SSL certificate for your wildcard custom domain in Azure Portal, whether by purchasing directly from Azure or by using an SSL certificate obtained from a third-party CA. This ensures secure communication between your BinaryPlate.BlazorPlate application and its users, enhancing privacy and data integrity.
