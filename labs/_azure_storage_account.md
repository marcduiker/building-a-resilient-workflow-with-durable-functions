# Azure Storage Account

Follow these steps to create an Storage Account in Azure if you can't use the Microsoft Azure Storage Emulator. An alternative would be to use the [Azure Portal](https://portal.azure.com).

## Using the Azure CLI

### Step 1. Login to Azure CLI

`az login`

### Step 2. See the available subscriptions
`az account list`

### Step 3. Set the desired subscription
`az account set -s "<id>"`

### Step 4. Create the Resource Group
`$location="westeurope"`

`$rgname="demo-neo-generator-rg"`

`az group create --name $rgname --location $location --tags type=demo`

### Step 5. Create the Storage Account
`$stname="neoprocessingstdemo"`

`az storage account create --name $stname --resource-group $rgname --location $location --sku Standard_LRS --kind StorageV2 --access-tier Hot`

### Step 6. Get the Connection String
`az storage account show-connection-string -n $stname -g $rgname`

You'll need to use this connectionstring in place of `UseDevelopmentStorage=true` in the `local.settings.json` file in later labs.