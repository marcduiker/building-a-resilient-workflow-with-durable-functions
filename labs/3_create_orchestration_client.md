# Lab 3 - Creating the orchestration client function

## Goal

The goal of this lab is to create an orchestration client which accepts the messages from the Service bus and starts a new instance of an orchestration. 

The orchestrator function can be left empty since that will be implemented in the next lab.

## Steps

### 1. Durable Functions extension

The Durable Function extension can be installed by including the following NuGet package:

`Microsoft.Azure.WebJobs.Extensions.DurableTask`

### 2. Update the Servicebus triggered function

Now change the signature of the Servicebus triggered function method to include the following parameter:

```csharp
[OrchestrationClient]DurableOrchestrationClientBase orchestratorClient
```

By adding this line to the function method the Durable Functions framework will inject an instance of the `DurableOrchestrationClient` which can be used to manage orchestration instances.

New orchestration function instances are started by passing in the name of the orchestration function to the `StartNewAsync` method like this:

```csharp
var instanceId = await orchestratorClient.StartNewAsync(
                    "NeoEventOrchestrator",
                    detectedNeoEvent)
```
The method returns the ID of the orchestration instance that is scheduled to start.

By using the syntax above update the function to start a new orchestration. **Do not create the actual `NeoEventOrchestrator` function just yet!**

Do a build of the project.

> Do you get any warnings about a missing orchestration function?

Now run/debug your local Function App. 

> What is the output from the Azure Functions Runtime in the console?

### 2. Create the orchestrator function

Now add a new class file for the orchestrator function.

### 3. Verify that the orchestration works

