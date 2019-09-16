# Lab 4 -  Adding the activity functions for the service calls

## Goal

The goal of this lab is to implement the activities which call out to existing services and call these from the orchestrator.

## Steps

### 1. Retrieving the Estimated Kinetic Energy

The first activity function will needs to make a call to an HTTP endpoint to retrieve the estimated kinetic energy expressed in megaton TNT.

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

- Install the `Microsoft.Extensions.Http` NuGet package. This contains the `IHttpClientFactory` interface.
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

The implementation of the function should be something like this:

```csharp
 var kineticEnergyEndpoint = new Uri(Environment.GetEnvironmentVariable("KineticEnergyEndpoint"));
 var apiManagementKey = Environment.GetEnvironmentVariable"ApiManagementKey");
 client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", piManagementKey);
 var response = await client.PostAsJsonAsynckineticEnergyEndpoint, neoEvent);
 var result = await esponse.Content.ReadAsAsync<KineticEnergyResult>();

 return result;
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

#### 1.4 Calling the activity from the orchestration

Let's return to the `NeoEventProcessingOrchestration` class and call the `EstimateKineticEnergyActivity` function.

The basic syntax for calling an activity is:

```csharp
var kineticEnergy = await context.CallActivityAsync<KineticEnergyResult>(
        nameof(EstimateKineticEnergyActivity),
        detectedNeoEvent);
```

#### 1.5 Dealing with failure

The syntax for calling an activity with retries is:

### 2. Retrieving the impact probability

Repeat the same substeps as in Step 1 but now for the `https://demo-neo.azure-api.net/neo/estimate/probability` endpoint.

### 3. Retrieving the Torino impact

Repeat the same substeps as in Step 1 but now for the `https://demo-neo.azure-api.net/neo/estimate/torino` endpoint.


### 4. Build en run locally

