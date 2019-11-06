# Lab 11 -  Create Azure resources

## Goal

The goal of this lab is to create the Azure resources which are required to run (and monitor) your Function App. 

You'll need an Azure account (see [the prerequisites](0_prerequisites.md)) in order to complete this lab.

## Steps

### 1. Azure Resources

Up to now you've ran your Function App locally using an emulated storage account. In order to run the application in Azure you need the following:

- A resource group, which is a logical container for your Azure resources
- A storage account, which is used to store your Function App files and is also used by Durable Functions.
- A Function App resource
- Optional but highly recommended: Application Insights, which is used for monitoring and diagnosing your application.

### 2. Creating resources

There are many differnt ways to create Azure resources; via the portal, the Azure CLI, ARM templates, and many cloud provision tools.

Here I'll show some samples how the Azure CLI can be used. But feel free to use the tools which work best for you.

In a command prompt type: `az login` in order to login with your Azure credentials.

> In the examples below I'm using a Powershell specific syntax for variables (e.g. `$location`) which are used in arguments. You need to change the syntax of these variables when you're not using Powershell.

> In order to see what options are available for a given CLI command use the `-h` argument, such as `az group -h` to see the available subcommands for resource groups.

Once logged in proceed with the following commands:

#### 2.1 See the available subscriptions
`az account list`

#### 2.2 Set the desired subscription
`az account set <id>`

#### 2.3 Create a resource group
`$location="westeurope"`

`$rgname="neo-processing-rg"`

`az group create --name $rgname --location $location --tags type=labs`

#### 2.4 Create a Storage Account  
`$stname="neoprocessingst"`

`az storage account create --name $stname --resource-group $rgname --location $location --sku Standard_LRS --kind StorageV2 --access-tier Hot`

#### 2.5 Add & Create application insights
Application Insights is not available by default in the Azure CLI and needs to be added first:

`az extension add --name application-insights`

`$ainame="neo-processing-ai"`

`az monitor app-insights component create --app $ainame --location $location --application-type web --kind web --resource-group $rgname`

#### 2.6 Create the Processing Function App
`$funcAppName="neo-processing-fa"`

`az functionapp create --name $funcAppName --resource-group $rgname --consumption-plan-location $location --storage-account $stname --app-insights $ainame --runtime dotnet --os-type Linux`

> Inspect the above CLI command. What can you tell about the configuration of the Function App?

Continue to the [next lab](12_publish_to_azure.md) to publish your Function App to Azure.
