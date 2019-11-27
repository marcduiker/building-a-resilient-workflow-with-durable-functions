# Lab 12 -  Publish your Function App to Azure

## Goal

The goals of this lab is to publish your application to Azure.

As with provisioning publishing your Function App can be done in different ways. You can do it straight from your IDE if that supports Azure (see [the prerequisites](00_prerequisites.md)), via the Azure Functions CLI, or via a build & release pipeline.

## Option 1: Publish from Visual Studio

Follow these instructions: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#publish-to-azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#publish-to-azure)

Note that you should select to publish to an existing Function App resource if you provisioned one in the prevous lab.

Application settings in the `local.settings.json` file are not published. Follow these instructions to manage your app settings from Visual Studio: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#function-app-settings](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs#function-app-settings)

## Option 2: Publish from VS Code

Follow these instructions: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code#publish-to-azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=nodejs#publish-to-azure)

Application settings in the `local.seetings.json` file are not published. Follow these instructions to manage your app settings from VS Code: [docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code#application-settings-in-azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=nodejs#application-settings-in-azure)

## Option 3: Publish from Azure Functions CLI

Follow these instructions: [docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#publish](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#publish)

## Option 4: Publish from Jetbrains Rider

Follow the instructions under the _Managing and deploying Azure Function apps with Rider_ section at [blog.jetbrains.com/dotnet/2019/05/09/building-azure-functions-sql-database-improvements-azure-toolkit-rider-2019-1/](https://blog.jetbrains.com/dotnet/2019/05/09/building-azure-functions-sql-database-improvements-azure-toolkit-rider-2019-1/)

## Monitor the Function App

Open the Azure portal, navigate to Application Insights and inspect how the Function App is performing.

## Get the orchestration status

Make a request to the Durable Functions HTTP management API to get the status of orchestrations which have been completed in the last 5 minutes.

An example request is available in [get_orchestration_status.http](../http/get_orchestration_status.http)

Continue to the [next lab](13_additional_features.md) to explore additional features.