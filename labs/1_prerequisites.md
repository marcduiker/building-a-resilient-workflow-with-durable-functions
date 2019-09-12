# Prerequisites

## Goal

The goal of this lab is to very that the required SDK, IDE and tools are installed.

## Steps

Verify that you have a recent .NET Core SDK installed:

- .NET Core SDK (at least 2.1.701). Check with typing `dotnet --list-sdks` in the console.
- .NET Core SDKs can be downloaded [here](https://dotnet.microsoft.com/download/dotnet-core).

Verify that you have an IDE with extension/plugins for Azure Functions:

- VS 2019 with Azure development workload
- VS Code with [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
- Jetbrains Rider 2019 with the Azure Toolkit Plugin

Verify that you have these tools installed in order to run Azure Functions locally:
- [Storage Emulator (Windows only)](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) OR
- [Azurite (VS Code extension)](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite)

Although most of the labs can completely done locally it would be fun if you could deploy your Function App to Azure in the final labs. For this you require:
- An [Azure account](https://azure.microsoft.com/en-us/free/).
- The [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) to create the Azure resources (or you can use the Azure portal to create these).