# Automating Deployment on Git Push to Master Using Azure Pipelines

## Introduction
This guide illustrates automating deployment on Git push events to the master branch using Azure Pipelines. Integrating deployment pipelines with version control triggers ensures rapid and reliable deployment of code changes to production environments, enhancing Continuous Deployment (CD) efficiency.

## Prerequisites
- Access to an Azure DevOps account.
- A Git repository hosted on GitHub.
- Visual Studio 2022 or any preferred code editor.
- Basic knowledge of Git and YAML.

## Step-by-Step Guide

### Step 1: Create an Azure DevOps Account
1. Visit the [Azure DevOps website](https://azure.microsoft.com/en-us/services/devops/) and click on "Start free with Azure DevOps."
2. Sign in with your Microsoft account or create a new one.
3. Follow the prompts to complete the sign-up process.

### Step 2: Create an Azure DevOps Project
1. Once logged in, navigate to the Azure DevOps dashboard.
2. Click on "Create Project" and provide a name for your project.
3. Choose version control settings (e.g., Git) and click "Create" to confirm.

### Step 3: Set Up Your GitHub Repository with Visual Studio
- Open Visual Studio 2022 or your preferred code editor.
- Access the Git Changes tab.
- Clone an existing repository if necessary.
- Add your project to version control.
- Commit your changes.
- Push your changes to GitHub.
- Optionally, create a new repository directly from Visual Studio.

### Step 4: Configure Your Deployment Pipeline in Azure DevOps
1. In your Azure DevOps project, go to Pipelines > Pipelines.
2. Click on "New Pipeline" to create a new pipeline.
3. Choose GitHub as the repository source.
4. Select your GitHub repository and click "Continue."
5. Configure your pipeline using the visual designer or YAML file.
6. Define stages, steps, and tasks for deployment to your target environment.

### Step 5: Configure Trigger for Git Push to Master
1. In the pipeline editor, navigate to the "Triggers" tab.
2. Enable the "Continuous deployment" option.
3. Click on "Add" to create a new trigger.
4. Choose "Branch filters" and specify the branch filter as "master."
5. Optionally, set additional filters or conditions such as paths or tags.
6. Configure any required security options or permissions.
7. Save your pipeline configuration.

### Step 6: Save and Run Your Deployment Pipeline
1. Save your pipeline configuration.
2. Optionally, trigger a manual run to verify functionality.
3. Azure Pipelines will automatically initiate deployments whenever changes are pushed to the master branch on GitHub.

## Additional Considerations
- Ensure proper configuration of deployment stages, steps, and tasks to facilitate deployment to the target environment.
- Utilize Azure Pipelines' deployment gates and approval workflows to add additional checks before deploying to production.
- Leverage Azure Pipelines' built-in integration with deployment targets such as Azure App Service, Kubernetes, or Virtual Machines for streamlined deployment workflows.

## Conclusion
By following this guide, developers can automate deployment processes on Git push events to the master branch using Azure Pipelines and GitHub. This integration enhances the efficiency of Continuous Deployment workflows, enabling rapid and reliable deployment of code changes to production environments.
