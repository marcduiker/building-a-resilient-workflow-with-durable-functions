# Lab 10 -  Unit testing

## Goal

The goal of this lab is to have your orchestrator function unit tested to gain confidence your business logic is implemented as expected. The logic in this case are statements in the orchestrator that controls when the `StoreProcessedNeoEventActivity` and the `SendNotificationActivity` are being called.

## Steps

### 1. Create a new unit test project

First add a new unit testing project to the solution (e.g. `Demo.NEO.EventProcessing.UnitTests`). I'm an xUnit enthusiast, but you can use any C# unit testing framework.

### 2. Mocking library

We want to test the functionality of the orchestrator function. This function has a dependency on the `IDurableOrchestrationContext` interface. We need to mock the behavior of this context so the actual activity functions are not executed and interfering with the unit tests.

You can use any mocking framework you like, some nice ones I can recommend are Moq, NSubstitute or FakteItEasy.

### 3. Create the unit tests

I suggest you write unit tests where you verify if the `CallActivityWithRetryAsync` method for the `StoreProcessedNeoEventActivity` and the `SendNotificationActivity` activities have been called.

In order to do this you need to create the `IDurableOrchestrationContext` mock and setup the method calls which are performed in the orchestrator function. I suggest you configure a so-called strict mock behavior which means that all methods on the `IDurableOrchestrationContext` which are used should be specified.

Note that method calls to the orchestrator and activities are async. This means your setup code and unit test methods should be able to handle this.

The unit tests should look something like shown below. Note that a new `DurableOrchestrationContextBaseBuilder` class is introduced which sets up the mock of the `IDurableOrchestrationContext`.

```csharp
[Fact]
public async Task WhenTorinoImpactIsGreaterThan0_ThenStoreProcessedNeoEventActivityShouldBeCalled()
{
    // Arrange
    var contextMock = DurableOrchestrationContextBaseBuilder.BuildContextWithSpecificTorinoImpactGreaterThan0();
    var logger = new Mock<ILogger>();
    var orchestrator = new NeoEventProcessingOrchestrator();
    
    // Act
    var orchestratorResult = await orchestrator.Run(contextMock.Object, logger.Object);
    
    // Assert
    contextMock.VerifyAll();
}
```

Continue to the [next lab](11_create_azure_resources.md) to create Azure resources for your Function App.
