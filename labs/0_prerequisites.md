# Lab 0.A - Prerequisites

## Goal

During this workshop we're going to write Azure Functions in C# (.NET Core). The goal of this lab is to very that the required SDKs, IDE and tooling are installed.

## Steps

### 1. SDKs

Verify that you have a recent .NET Core SDK installed:

- .NET Core SDK (at least 2.2+). Check with typing `dotnet --list-sdks` in the console.
- .NET Core SDKs can be downloaded [here](https://dotnet.microsoft.com/download/dotnet-core).

### 2. IDE & Extensions

Verify that you have an IDE with extension/plugins for Azure Functions:

- VS 2019 with Azure development workload
- VS Code with [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
- Jetbrains Rider 2019 with the Azure Toolkit Plugin

### 3. Tooling

Verify that you have these tools installed in order to run Azure Functions locally:
- [Storage Emulator (Windows only)](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) OR
- [Azurite (VS Code extension)](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite)
- Optional: [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)
- [VS Code + REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) or Postman in order to make HTTP requests.

### 4. Azure account & Azure CLI

Although most of the labs can be done completely locally it would be fun if you could deploy your Function App to Azure in the final labs. For this you require:
- An [Azure account](https://azure.microsoft.com/en-us/free/).
- The [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) to create the Azure resources (or you can use the Azure portal to create these).

### 5. Code snippets

Completely optional but still useful:
- I have made some [code snippets](https://github.com/marcduiker/durable-functions-snippets), which can speed up the process (and prevent mistakes) when creating client, orchestrator and activity functions.

Now let's move on to [next lab](0_subscribe.md) to get your subscription keys and connectionstring.