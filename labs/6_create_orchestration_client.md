# Lab 2 - Creating the orchestration client function

## Goal

The goal of this lab is to create an orchestration client function which accepts the detected NEO messages from the Servicebus and starts a new instance of an orchestration. The orchestrator function will be created in the next lab and should not be created as part of this lab.

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
      "hubName": "NEOEventsV1"
    }
  }
}
```

> For more info about storage and task hubs please read the [Task hubs in Durable Functions](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-task-hubsx) documentation.

> **Since you'll be making a lot of (breaking) changes to the orchestration in the next labs, I suggest you add a version suffix to the hubName each time you make a change to the orchestrator code. By doing this, Durable Functions will create new tables & queues, and you won't run into issues running new orchestrator code with old data.**

### 3. Update the Servicebus triggered function

Now update the signature of the Servicebus triggered function method and add the following parameter to give it an orchestration client function role:

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

By using the syntax above update the function to start a new instance of the NeoEventProcessingOrchestrator function. **Do not create the actual `NeoEventProcessingOrchestrator` function just yet!**

> Note that the method is asynchronous and the result is awaited. You probably need to update your function signature to make it `async` and return a `Task`.

### 4. Build & run locally

Do a build of the project.

> Do you get any warnings about a missing orchestration function?

Now run/debug your local Function App. 

> What is the output from the Azure Functions Runtime in the console once the function is triggered? What does the failure say?

### 5. More control when debugging locally

Messages are continuously being pushed to the Servicebus topic. This makes it quite difficult for you when you're debugging because new orchestrators are being instantiated every couple of seconds.

Lets disable the ServiecbusTrigger for now and create a complementary HttpTrigger which you can start yourself.

#### Disabling the ServicebusTrigger function

Functions can be disabled by adding this line to the application settings in `local.settings.json`:

```json
"AzureWebJobs.<FUNCTION_NAME>.Disabled": true
```

#### Adding an HttpTrigger function

Now add an HttpTrigger function, which responds to a POST to the `api/start` route. The function should extract the `DetectedNeoEvent` from the message body and start a new orchestator function in the same way as the ServicebusTrigger function.

Since the trigger is now Http based we can return an `HttpResponseMessage` with the ID of the started orchestration.

- Change the return type to `Task<HttpResonseMessage>`
- Use the `CreateCheckStatusResponse` method to return the status response object.

The implementation should look something like this:

```csharp
[FunctionName(nameof(NeoEventProcessingClientHttp))]
public async Task<HttpResponseMessage> Run(
    [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "start")]HttpRequestMessage message,
    [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
    ILogger log)
{
    var detectedNeoEvent = await message.Content.ReadAsAsync<DetectedNeoEvent>();
    var instanceId = await orchestrationClient.StartNewAsync("NeoEventProcessingOrchestrator"),
        detectedNeoEvent);

    log.LogInformation($"HTTP started orchestration with ID {instanceId}.");

    return orchestrationClient.CreateCheckStatusResponse(message, instanceId);
}
```

### 6. Test the HttpTrigger client function

Now trigger the HttpTrigger client function by using the request in the [start_orchestration.http](../http/start_orchestration.http) file. You can direcly execute the requests in this file by using VSCode and the REST Client extension.

[Durable Functions has an HTTP API](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-http-api) which allows management of the orchestrator instances. This can be very useful for debugging and maintenance. The [start_orchestration.http](../http/start_orchestration.http) file also contains two requests to get the result of orchestrations.

Continue to the [next lab](3_create_orchestrator_function.md) to create an orchestrator function.
