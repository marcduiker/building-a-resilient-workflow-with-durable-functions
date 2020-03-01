# Lab 11 - Fan-out/Fan-in

## Goal

The goal of this lab is to apply the fan-out/fan-in pattern in order to perform activity functions in parallel.

## Steps

The first two activity functions in the `NeoEventProcessingOrchestrator` can be executed in parallel. We have to wait until the result from both functions are available before we construct the `TorinoImpactRequest`.

### 1. Change to return tasks

Open the `NeoEventProcessingOrchestrator` orchestrator function and locate the first two activity functions:

- Remove the `await` keyword before the `context.CallActivity...` methods.
- Rename the variables which captures the output from the activity calls and postfix them with the word `task`. In my case I have variables `kineticEnergyTask` and `impactProbabilityTask`.

### 2. Await both tasks

Before constructing the `TorinoImpactRequest` object, add this:

```csharp
await Task.WhenAll(kineticEnergyTask, impactProbabilityTask);
```

This ensures that the orchestrator code will only continue executing when all the tasks have been completed.

You can access the kinectic energy and impact probability by using the `Result` property on the respective tasks.

### 3. Build & run locally

Now run/debug your local Function App again. No functional change should be visible but now the first two activities are executed in parallel.

Continue to the [next lab](12_suborchestrations.md) to use suborchestrations.
