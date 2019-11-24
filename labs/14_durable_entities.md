# Lab 14 - Durable Entities

## Goal

The goal of this lab is to add the functionality to keep track how many NEO events have been processed. 
This can be realized using a Durable Entities, which is part of Durable Functions since v2.
More information about Durable Entities are available in [the official docs](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-dotnet-entities).

## Steps

There are two ways to use the Durable Entities API, the class based syntax, and the function based syntax. The class based option offers the most type-safety, so this approach is used in here.

### 1. Adding the `IProcessedNeoEventCounter` Interface

Start by adding the following interface to the EventProcessing Function App.

```csharp
public interface IProcessedNeoEventCounter
{
    void Add();
    Task<int> GetAsync();
}
```

The methods will allow us the increment the counter through the `Add()` method and to retrieve the counter value using the `GetAsync()` method.

There are quite some restrictions on Durable Entity interfaces, please see the [documentation](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-dotnet-entities#restrictions-on-entity-interfaces).

### 2. Adding the `ProcessedNeoEventCounter` Entity

Continue with adding the implementation for the interface:

```csharp
[JsonObject(MemberSerialization.OptIn)]
public class ProcessedNeoEventCounter : IProcessedNeoEventCounter
{
    [JsonProperty("currentCount")]
    public int CurrentCount { get; set; }

    public void Add() => CurrentCount += 1;
    
    public Task<int> GetAsync() => Task.FromResult(CurrentCount);
    
    [FunctionName(nameof(ProcessedNeoEventCounter))]
    public static Task Run([EntityTrigger] IDurableEntityContext ctx)
        => ctx.DispatchAsync<ProcessedNeoEventCounter>();
}
```

Well, this doesn't look anything like a function which you've make before, but you've just wrote your first Durable Entity class!

The state is kept in the `CurrentCount` property and is persisted as JSON by the Durable Functions extension without you having to worry about it. 

Entity classes also have some requirements as is described in [the documentation](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-dotnet-entities#class-requirements).

### 3. Extending the NeoEventProcessingOrchestrator

We want to count every `ProcessedNeoEvent`. Therefore put the following code in the `NeoEventProcessingOrchestrator` class before the statement which controls if the `StoreProcessedNeoEventActivity` will be called.

```csharp
var proxy = context.CreateEntityProxy<IProcessedNeoEventCounter>(
        EntityIdBuilder.BuildForProcessedNeoEventCounter());
proxy.Add();
```

Note that we can use the `IDurableOrchestrationContext` since that exposes the `CreateEntityProxy<T>()` method. Once we have the proxy, the `Add()` method is called to increment the counter. Orchestrations and Entities work really well together.

The `EntityId` is created by using a seperate builder class since it will be used in more places:

```csharp
public static class EntityIdBuilder
{
    public static EntityId BuildForProcessedNeoEventCounter()
    {
        return new EntityId(nameof(ProcessedNeoEventCounter), "counter");
    }
}
```

> What exactly are the arguments when an EntityId is constructed? How do you think they are used?

### 4. Updating the Mock for the Unit Tests

If you have created unit tests and are using strict mocks your tests will now fail now because not all methods have a matching setup.

The `CreateEntityProxy` method is an extension method and cannot be setup. The result of this method is a proxy, which enables us to use the methods which are known in the interface. Under the hood however, these calls translate to a `SignalEntity()` method on the context which accepts an `EntityId`, an operationName (`string`) and an operationInput (`object`). Therefore the `IDurableOrchestrationContext` mock should be extended with the following setup:

```csharp
contextMock.Setup(ctx => ctx.SignalEntity(
        It.IsAny<EntityId>(), 
        nameof(ProcessedNeoEventCounter.Add),
        null));
```

Now the unit tests should pass again.

### 5. Retrieving the Count

We've implemented the counting of processed NEOEvents but we don't have the functionality to retrieve this count.

You can do this by adding an HttpTrigger function which also uses the `DurableClient` attribute:

```csharp
[FunctionName(nameof(ProcessedNeoEventCounterHttp))]
public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req,
    [DurableClient] IDurableClient durableClient,
    ILogger log)
{
    var neoEventCountEntity = await durableClient.ReadEntityStateAsync<ProcessedNeoEventCounter>(
        EntityIdBuilder.BuildForProcessedNeoEventCounter());
    if (neoEventCountEntity.EntityExists)
    {
        var count = await neoEventCountEntity.EntityState.GetAsync();
        return new OkObjectResult($"{count} NEO events have been processed.");
    }
    
    return new NotFoundObjectResult("The IProcessedNeoEventCounter entity was not found.");
}
```

In the code above, the `ProcessedNeoEventCounter` entity is retrieved from storage. If it exists the count is retreived by calling the `GetAsync()` method.

> What could be the reason for an entity not to exist?

### 6. Build & Run Locally

Now run/debug your local Function App. You might want to disable the ServicebusTrigger again in the `local.settings.json`. 

You can use [this http request](../http/processed_neo_events_counter.http) to trigger the ProcessedNeoEventCounterHttp function in order to retrieve the count.

Use the [HttpTrigger client function](../http/start_orchestration.http) to trigger the orchestration manually. Trigger the orchestration a couple of times and verify with the ProcessedNeoEventCounterHttp function that the count is working.