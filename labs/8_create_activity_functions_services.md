# Lab 4 -  Creating the activity functions for the service calls

## Goal

The goal of this lab is to implement the activities which call out to existing XASA services and call these from the orchestrator.

## Steps

### 1. Retrieving the Estimated Kinetic Energy

The first activity function needs to make a call to an HTTP endpoint to retrieve the estimated kinetic energy expressed in megaton TNT.

An HTTP POST need to be done to `https://demo-neo.azure-api.net/neo/estimate/energy` with a payload of the `DetectedNEOEvent` object in the body. A `KineticEnergyResult` object will be returned.

#### 1.1  Adding a new class for the Estimated Kinetic Energy service call

Add a new class, `EstimateKineticEnergyActivity`, and add the following method definition which makes it an activity function:

```csharp
[FunctionName(nameof(EstimateKineticEnergyActivity))]
public async Task<KineticEnergyResult> Run(
    [ActivityTrigger] DetectedNeoEvent neoEvent,
    ILogger logger)
{
}
```

#### 1.2 Injecting the IHTTPClientFactory

In order to make HTTP calls we require a `HttpClient` object. The 'old way' of doing this was to use a static private `HttpClient` field. However, since Azure Functions now supports dependency injection we can inject an `IHttpClientFactory` into this class and the factory manages the lifetime of the `HttpClient`.

- Install the `Microsoft.Extensions.Http` NuGet package (__v 2.2.0__). This contains the `IHttpClientFactory` interface.
- Add a constructor to the `EstimateKineticEnergyActivity` class and inject `IHttpClientFactory`.
- Add a private readonly field of type `HttpClient` to the class.
- Set this field in the constructor by using the `CreateClient()` method of the `IHttpClientFactory`.

```csharp
private readonly HttpClient client;

public EstimateKineticEnergyActivity(IHttpClientFactory httpClientFactory)
{
    client = httpClientFactory.CreateClient();
}
```

Now the client can be used in the function method to do a `PostAsJsonAsync()` call to the EstimateKineticEnergy endpoint. Note that you also need to pass in the ApiManagementKey in the `Ocp-Apim-Subscription-Key` header.

When you do the post to the endpoint, make sure you verify on a success status code on the response. If the response is not successful an exception should be thrown from the activity.

The implementation of the function should be something like this:

```csharp
var kineticEnergyEndpoint = new Uri(Environment.GetEnvironmentVariable("KineticEnergyEndpoint"));
var apiManagementKey = Environment.GetEnvironmentVariable("ApiManagementKey");
client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiManagementKey);
var response = await client.PostAsJsonAsync(kineticEnergyEndpoint, neoEvent);
if (!response.IsSuccessStatusCode)
{
    var content = await response.Content.ReadAsStringAsync();
    throw new FunctionFailedException(content);
}
var result = await response.Content.ReadAsAsync<KineticEnergyResult>();

return result;
```

#### 1.4 Calling the activity from the orchestration

Let's return to the `NeoEventProcessingOrchestrator` class and call the `EstimateKineticEnergyActivity` function.

The basic syntax for calling an activity with a return type is:

```csharp
var kineticEnergy = await context.CallActivityAsync<KineticEnergyResult>(
        nameof(EstimateKineticEnergyActivity),
        detectedNeoEvent);
```

If you'd run the Function App now, you'll get exceptions (like the one below) since the `IHttpClientFactory` dependency has not been registered yet.

```
Microsoft.Extensions.DependencyInjection.Abstractions: Unable to resolve service for type 'System.Net.Http.IHttpClientFactory' while attempting to activate '<NAME_OF_FUNCTION>'.
```

#### 1.3 Registering the HTTPClientFactory in Startup

In order to use dependency injection in Azure Functions you need to go through the following steps:

- Add a reference to the `Microsoft.Azure.Functions.Extensions` NuGet package.
- Add a new class (e.g Startup.cs) to the Function App and inherit from `FunctionsStartup`.
- Add the `FunctionsStartup` attribute on the assembly level and provide the type of the Startup class as the argument.
- Implement the `Configure` method and use the `AddHttpClient()` extension method on the `builder.Services`.

The result should look like this:

```csharp
[assembly: FunctionsStartup(typeof(Startup))]
namespace Demo.NEO.EventProcessing.Application
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
        }
    }
}
```

> If you run the Function App for a while you will notice (in the Azure Functions runtime output) that the activity function fails in some occasions. Why is that?

#### 1.5 Dealing with failure

A good solution to cope with failing activity functions in this case is to retry the activity. The Durable Functions API already has built-in support for this using the `CallActivityWithRetryAsync` method:

```csharp
var kineticEnergy = await context.CallActivityWithRetryAsync<KineticEnergyResult>(
        nameof(EstimateKineticEnergyActivity),
        new RetryOptions(TimeSpan.FromSeconds(10), 5), 
        detectedNeoEvent);
```

> Notice the different method name and the `RetryOptions` argument. Go to the definition of the `RetryOptions` type to see which properties are available.

Now update the orchestrator function with the `CallActivityWithRetryAsync` method and run the Function App.

### 2. Retrieving the Impact Probability

Repeat the same substeps as in Step 1 (excl the Startup class) but now to do a post to the `https://demo-neo.azure-api.net/neo/estimate/probability` endpoint. This also uses a `DetectedNeoEvent` as the body and uses the same `Ocp-Apim-Subscription-Key` header. An `ImpactProbabilityResult` object will be returned.

### 3. Retrieving the Torino impact

Repeat the same substeps as in Step 1 (excl the Startup class) but now to do a post to the `https://demo-neo.azure-api.net/neo/estimate/torino` endpoint. This requires a `TorinoImpactRequest` object as the body. The request can be built by combining the `KineticEnergyResult` and `ImpactProbabilityResult` objects.

 The TorinoImpact endpoint returns an `TorinoImpactResult`. Again the same `Ocp-Apim-Subscription-Key` header is used for authentication.

### 4. Return the ProcessedNeoEvent

The users of your application are interested in `ProcessedNeoEvent` objects. These are objects built up of DetectedNeoEvents and the results from the three service calls you just implemented.

Once all three activities have been implemented create a new instance of the `ProcessedNeoEvent` and for now return this from the orchestrator.

Now run/debug your local Function App by using the [HttpTrigger client function](../http/start_orchestration.http).

Continue to the [next lab](5_create_activity_function_storage.md) to create the activity to store the processed data.
