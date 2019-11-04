# Lab 3 - Creating the orchestrator function

## Goal

The goal of this lab is to create a basic orchestrator function which will orchestrate the activities that will be implemented in the next labs.

The orchestrator should do the following steps:
- Retrieve the `DetectedNeoEvent` input (which is passed in by the orchestation client).
- Retrieve the kinetic energy for the object.
- Retrieve the the impact probability for the object.
- Retrieve the Torino impact for the object.
- Save the `ProcessedNeoEvent` result to blob storage for events with a Torino impact >= 1.
- Send a notification for events with a Torino impact >= 8.

## Steps

### 1. Creating an orchestrator function

Create a new class (`NeoEventProcessingOrchestrator`) with a function definition as follows:

```csharp
[FunctionName(nameof(NeoEventProcessingOrchestrator))]
public async Task Run(
    [OrchestrationTrigger] DurableOrchestrationContextBase context,
    ILogger logger)
    {
    }
```

### 2. Get the input

Use the `GetInput` method on the context to retrieve the typed object.

```csharp
var detectedNeoEvent = context.GetInput<DetectedNeoEvent>();
```

Remember that the state of orchestrator functions is checkpointed to storage and the code of the orchestrator function will be replayed several times. The code you write in this orchestrator [should be deterministic](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-code-constraints) so it doesn't affect the replay.

### 3. Build & run locally

Now run/debug your local Function App by using the [HttpTrigger client function](../http/start_orchestration.http).

> Is the orchestrator function detected by the Azure Functions runtime?

> What is the output from the Azure Functions runtime in the console once the function is triggered? Do you see any sign that the orchestrator function is being executed?

Continue to the [next lab](4_create_activity_functions_services.md) to create activity functions.