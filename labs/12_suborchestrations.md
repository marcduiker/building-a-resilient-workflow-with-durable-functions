# Lab 12 - Suborchestrations

## Goal

The goal of this lab is to move some activities from the main orchestrator into a new orchestrator (aka suborchestrator) which will be called from the main one.

At a certain moment orchestrators can become too big and will be difficult to understand and unittest. We can put activities which belong together in another orchestrator so both the new orchestrator and the old orchestrator will be easier to understand.

## Steps

When we look at the `NeoEventProcessingOrchestrator` orchestrator the first 3 activitites deal with calculations and determining the Torino impact. The other two activities deal with storage and notification. The activities required for determine the Torino impact look like a good candidate to extract to their own orchestrator. 

### 1. Create a new orchestrator class

Create a new class called `TorinoImpactCalculationOrchestrator` and add a function definition for an orchestrator function. We can look at [lab 6](06_create_orchestrator_function.md) how that was done.

I suggest the ouput of this orchestrator is an `ProcessedNeoEvent` object. The input can be the `DetectedNeoEvent` object.

### 2. Move the activities

Now copy the activities from `NeoEventProcessingOrchestrator` and paste them in `TorinoImpactCalculationOrchestrator` inluding the code that combines the results and creates an instance of a `ProcessedNeoEvent`.

### 3. Call the suborchestrator

In the `NeoEventProcessingOrchestrator` call the `TorinoImpactCalculationOrchestrator` suborchestrator as follows:

```csharp
var processedNeoEvent = await context.CallSubOrchestratorWithRetryAsync<ProcessedNeoEvent>(
        nameof(TorinoImpactCalculationOrchestrator),
        GetRetryOptions(),
        detectedNeoEvent);
```

### 4. Fix the unit tests

If we wrote unit tests for test the `NeoEventProcessingOrchestrator` it should break now since some activities are not used any longer in this orchestrator and a call to a suborchestrator is done. Remove the setup for calling those activity functions and add a setup for calling the suborchestrator.

### 5. Build & run locally

Now run/debug your local Function App again. No functional change should be visible but the `NeoEventProcessingOrchestrator` will now call the `TorinoImpactCalculationOrchestrator` and wait for its result.

Continue to the [next lab](13_external_events.md) to wait for external events.
