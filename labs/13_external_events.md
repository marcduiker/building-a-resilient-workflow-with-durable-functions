# Lab 13 - External Events

## Goal

The goal of this lab is only send the email when an approval event is received from a client. More information about external events can be read in the [official documentation](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-external-events?tabs=csharp).

## Steps

The solution consists of two parts, handling the event in the orchestrator and sending the event. The sending of the event can be done through code in an orchestration client function or through an Http call to the Durable Function Http API.

### 1. Waiting for an event

Open the `NeoEventProcessingOrchestrator` orchestrator function add this code before the `SendNotificationActivity` activity is called.

```csharp
var approvalResult = await context.WaitForExternalEvent<bool>(
        "ApprovalEvent",
        TimeSpan.FromHours(1),
        false);
```

> Make yourself familiar with the inputs and output of this method. In this case, the result is a boolean. The orchestrator waits until an event named `"ApprovalEvent"` is raised. In case the event has not been received within 1 hour since the start of the orchestrator, the default value (`false`) is used as the output.

Now, add an if statement to the orchestrator to only call the `SendNotificationActivity` when `approvalResult` is `true`.

### 2. Raising an event via the DurableFunctions Http API

You can use the Http call defined below to raise an event. 

```http
POST <rooturl>/runtime/webhooks/durabletask/instances/{instanceId}/raiseEvent/{eventName}
    ?taskHub={taskHub}
Content-Type: application/json

{content}
```

Information about the parameters can be found in the [official docs](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-http-api#raise-event). 


### 3. Raising an event via an orchestration client function

The other was to raise an event is to use C# API in a client function.

- Create a new Http triggered function in the Function App.
- Our method to raise an event requires an orchestration instance ID and a boolean value. Ensure we can pass these values either through the route, query string paramters or the body of the request. 
- Add an argument to the method which uses the `[DurableClient]` attribute and `IDurableClient` interface [as done before in lab 5](05_create_orchestration_client.md)
- Add the following code in the body of the method:

```csharp
await client.RaiseEventAsync(
    instanceId,
    "ApprovalEvent",
    isApproved);
```

### 4. Add/update unit tests

We can now update the unit tests for the `NeoEventProcessingOrchestrator` and specify cases the aproval result to be `true` and `false`.

### 5. Build & run locally

Now run/debug your local Function App using the Http trigger so we can force a Torino impact of 8. Once an instance of the orchestrator has started, use that instanceId to raise an approval event either through the built-in Http API or the custom Http triggered client function.

Continue to the [next lab](14_durable_entities.md) to count the events using durable entities.
