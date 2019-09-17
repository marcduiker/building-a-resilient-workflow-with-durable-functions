# Lab 0 - Prerequisites

## Goal

During this workshop we're going to write Azure Functions in C# (.NET Core). The goal of this lab is to very that the required SDKs, IDE and tooling are installed.

## Steps

### 1. Clone or fork this repo

Clone this repo so you can reference the `Demo.NEO.Models.csproj` project which is required in later labs. When you're about to create your own Function App, add it somewhere inside the `/src` folder in this repo.

My final implementation is also in this repo. You can use it in case you get stuck. But before you look at my implementation, please ask for help from the person next to you (or better do some pair programming).

### 2. SDKs

Verify that you have a recent .NET Core SDK installed:

- .NET Core SDK (at least 2.2+). Check with typing `dotnet --list-sdks` in the console.
- .NET Core SDKs can be downloaded [here](https://dotnet.microsoft.com/download/dotnet-core).

### 3. IDE & Extensions

Verify that you have an IDE with extension/plugins for Azure Functions:

- VS 2019 with Azure development workload
- VS Code with [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
- Jetbrains Rider 2019 with the Azure Toolkit Plugin

### 4. Tooling

Verify that you have these tools installed in order to run Azure Functions locally:
- [Storage Emulator (Windows only)](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) OR
- [Azurite (VS Code extension)](https://marketplace.visualstudio.com/items?itemName=Azurite.azurite)

### 5. Azure account & Azure CLI

Although most of the labs can completely done locally it would be fun if you could deploy your Function App to Azure in the final labs. For this you require:
- An [Azure account](https://azure.microsoft.com/en-us/free/).
- The [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) to create the Azure resources (or you can use the Azure portal to create these).

### 6. Code snippets

Completely optional but still useful:
- I have made some [code snippets](https://github.com/marcduiker/durable-functions-snippets), which can speed up the process (and prevent mistakes) when creating client, orchestrator and activity functions.

Get started with the [next lab](1_creating_a_function_project.md) to create a Function App.