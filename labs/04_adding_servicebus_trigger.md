# Lab 4 - Creating a Function App Project

## Goal

The goal of this lab is to create a basic Function App with a Servicebus triggered function and to determine it is connected correctly and receiving detected NEO messages from the topic.

## Steps

### 1. Create a Function App with a ServiceBusTrigger function

With your IDE of choice create a Function App (.NET Core v3) (suggested name: `NeoEventProcessing`) with a Servicebus Topic triggered function. The suggested name for the function, and the class, is `NeoEventProcessingClientServicebus`.

- When asked, specify that we want to use the Storage Emulator which is used to run the app locally.
- Specify the following Servicebus settings in the `ServiceBusTrigger` attribute:

    -   Connectionstring setting name: `NEOEventsTopic`
    -   Topic name: `neo-events`
    -   Subscription name: `<subscriptionname>` (get this from the blob json file that is created in the earlier previous lab)

The resulting servicebus function trigger should look something like this:

```csharp
[ServiceBusTrigger("neo-events", "<YOUR_PERSONAL_TOPIC_SUBSCRIPTIONNAME>", Connection = "NEOEventsTopic")]string message, 
```

The trigger now has a connection name which will be looked up in the application settings. But there's is no actual connectionstring specified yet.

Locate the `local.settings.json` file in your local function folder and add the connection name (`NEOEventsTopic`) and the connectionstring (output from the previous lab):

```json
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_V2_COMPATIBILITY_MODE": true,
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "NEOEventsTopic": "<servicebus_connectionstring>"
  }
}
```

Also make sure to add the `FUNCTIONS_V2_COMPATIBILITY_MODE:true` setting. This ensures correct json (de)serialization, which is an unresolved issue in Azure Functions v3 at the moment.

### 2. Run the Function App locally

Now that your Function App is configured to receive messages let's try and run it locally to verify it does. Add a breakpoint to the Servicebus triggered function and run/debug your Function App locally.

> The Azure Function Runtime should start in a console window and show some diagnostic information. If there are any issues with storage and/or service bus configuration we will see the errors in this console. 

> If you run into service bus connection issues it could be that your local machine has firewall rules in place which prohibit the connection to the service bus endpoint. If this is the case, first follow step 3 in order to use the `Demo.NEO.Models` NuGet package and then continue with Lab 5, step 5, which disables the ServicebusTrigger and continues with an HttpTrigger.

> What is format of the message we're receiving?

> Are there other types which are also acceptable instead of `string`?

### 3. Convert the messages to DetectedNeoEvent objects

Currently the inconming mesage is of type string and the content is json. Let's convert this to the strongly typed model so we can work with the data more easily.

The `DetectedNeoEvent` type is part of this NuGet package: `Demo.NEO.Models`. You can download the package by connecting to this NuGet feed:

`https://pkgs.dev.azure.com/marcduiker/Building Resilient Workflows With Durable Functions/_packaging/Public/nuget/v3/index.json`

If you aren't configuring the feed using an IDE you can also add a file named `nuget.config` to the root of your solution with the following content:

```xml
<configuration>
  <packageSources>
    <add key="Xasa" value="https://pkgs.dev.azure.com/marcduiker/Building Resilient Workflows With Durable Functions/_packaging/Public/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

Once you have added the `Demo.NEO.Models` package to the project change the type of the message argument to `DetectedNeoEvent`:

```csharp
[ServiceBusTrigger("neo-events", "<YOUR SUBSCRIPTION KEY>", Connection = "NEOEventsTopic")]DetectedNeoEvent detectedNeoEvent,
```

### 4. Run the Function App locally

Run your Function App locally and verify we're receiving `DetectedNeoEvent` objects.

### 5. Optional: Moving Attribute Parameters to App Settings

When we look at the `ServiceBusTrigger` attribute the topic name and the subscription name are hardcoded in the attribute parameters. You can move the actual values to the `local.settings.json` file as key-value pairs and reference the settings in the attribute by using `%KEY%`as the parameter value.

```csharp
[ServiceBusTrigger("%TopicName%", "%SubscriptionName%", Connection = "NEOEventsTopic")]DetectedNeoEvent detectedNeoEvent, 
```

If everything works as expected continue with the [next lab](05_create_orchestration_client.md) to create an orchestration client.