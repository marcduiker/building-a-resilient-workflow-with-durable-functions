# Lab 1 - Creating a Function App Project

## Goal

The goal of this lab is to create a basic Function App with a Servicebus triggered function and to determine it is connected correctly and receiving detected NEO messages from the topic.

## Steps

### 1. Create a Function App with a ServiceBusTrigger function

With your IDE of choice create a Function App (suggested name: `NeoEventProcessing`) with a Servicebus Topic triggered function. The suggested name for the function, and the class, is `NeoEventProcessingClientServicebus`.

- When asked, specify that you want to use the Storage Emulator which is used to run the app locally.
- Specify the following Servicebus settings in the `ServiceBusTrigger` attribute:

    -   Connectionstring setting name: `NEOEventsTopic`
    -   Topic name: `neo-events`
    -   Subscription name: `<subscriptionname>` (get this from the blob json file that is created in the earlier previous lab)

The resulting servicebus function trigger should look something like this:

```csharp
[ServiceBusTrigger("neo-events", "<subscriptionname>", Connection = "NEOEventsTopic")]string message, 
```

The trigger now has a connection name which will be looked up in the application settings. But there's is no actual connectionstring specified yet. 

Add the connection name and the actual connectionstring (from the json file) to the `local.settings.json` file in your local function folder:

```json
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "NEOEventsTopic": "<servicebus_connectionstring>"
  }
}
```

### 2. Run the Function App locally

Now that your Function App is configured to receive messages let's try and run it locally to verify it does. Add a breakpoint to the Servicebus triggered function and run/debug your Function App locally.

> The Azure Function Runtime should start in a console window and show some diagnostic information. If there are any issues with storage and/or service bus configuration you will see the errors in this console. 

> What is format of the message you're receiving?

> Are there other types which are also acceptable instead of `string`?

### 3. Convert the messages to DetectedNeoEvent objects

Currently the inconming mesage is of type string and the content is json. Let's convert this to the strongly typed model so we can work with the data more easily.

The `DetectedNeoEvent` type is part of this NuGet package: `Demo.NEO.Models`. You can download the package by connecting to this NuGet feed:

`https://pkgs.dev.azure.com/marcduiker/Building Resilient Workflows With Durable Functions/_packaging/Public/nuget/v3/index.json`

Add the following line to convert to the strongly typed object. 

```csharp
var detectedNeoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(message);
```

When you need to add a reference to NewtonSoft.Json, don't add the latest version but use version 11.0.2 (the version the Azure Functions Runtime is also using).

### 4. Run the Function App locally

Again run your Function App locally and verify you can convert the messages to `DetectedNeoEvent` objects.

If everything works as expected continue with the [next lab](5_create_orchestration_client.md) to create an orchestration client.