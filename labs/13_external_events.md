# Lab 13 - External Events

## Goal

The goal of this lab is only send the email when an approval event is received from a client. More information about external events can be read in the [official documentation](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-sub-orchestrations?tabs=csharp).

## Steps

The solution consists of two parts, handling the event in the orchestrator and sending the event. The sending of the event can be done through code in an orchestration client function or through an Http call to the Durable Function Http API.

### 1. Waiting for an event

Open the `NeoEventProcessingOrchestrator` orchestrator function and locate the first two activity functions:

### 2. Raising an event via the Http API


### 3. Raising an event via an orchestration client function 

### 4. Add/update unit tests

We can now update the unit tests for the `NeoEventProcessingOrchestrator` and specify cases the aproval event to be `true` and `false`. 

### 5. Build & run locally

Now run/debug your local Function App again. No functional change should be visible but now the first two activities are executed in parallel.

Continue to the [next lab](14_durable_entities.md) to count the events using durable entities.
