# Lab 2 - Creating the orchestration client function

## Goal

The goal of this lab is to create an orchestration client which accepts the messages from the Service bus and starts a new instance of an orchestration.

The orchestrator function can be left empty since that will be implemented in the next lab.

## Steps

### 1. Reference the Durable Functions extension

In order to use the Durable Functions API you need a reference to this NuGet package:

`Microsoft.Azure.WebJobs.Extensions.DurableTask`

### 2. Specify a task hub name

Durable Functions uses Table Storage to checkpoint the state of the orchestration and Storage Queues to schedule the orchestrator and its activities. You have control over the prefix used in naming these tables and queues by specifying the hubName setting in the `host.json` file:

```json
{
  "version": "2.0",
  "extensions": {
    "durableTask": {
      "hubName": "NEOEvents"
    }
  }
}
```

> For more info about Storage and Task Hubs please read the [Task hubs in Durable Functions](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-task-hubsx) documentation.

### 3. Update the Servicebus triggered function

Now update the signature of the Servicebus triggered function method to add the following parameter to give it an orchestration client function role:

```csharp
[OrchestrationClient]DurableOrchestrationClientBase orchestratorClient
```

> If you installed a (preview) 2.x version of Durable Functions extension there are no base classes anymore. Use the `IDurableOrchestrationClient` interface instead.

By adding this line to the function method the Durable Functions framework will inject an instance of the `DurableOrchestrationClient` which is used to manage orchestration instances.

New orchestration function instances are started by passing in the name of the orchestration function to the `StartNewAsync` method like this:

```csharp
var instanceId = await orchestratorClient.StartNewAsync(
                    "NeoEventProcessingOrchestrator",
                    detectedNeoEvent)
```
The method returns the ID of the orchestration instance that is scheduled to start.

By using the syntax above update the function to start a new orchestration. **Do not create the actual `NeoEventOrchestrator` function just yet!**

> Note that the method is asynchronous and we are awaiting the result. You probably need to update your function signature to make it `async` and return a `Task`.

### 4. Build & run locally

Do a build of the project.

> Do you get any warnings about a missing orchestration function?

Now run/debug your local Function App. 

> What is the output from the Azure Functions Runtime in the console once the function is triggered? What does the failure say?

Continue to the [next lab](3_add_orchestrator_function.md) to create an orchestrator function.

