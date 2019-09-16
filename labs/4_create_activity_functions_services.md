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

In order to make HTTP calls we require a `HttpClient` object. Since Azure Functions supports dependency injection we can inject an `IHttpClientFactory` into this class. 

- Install the `Microsoft.Extensions.Http` NuGet package.
- Add a constructor to the `EstimateKineticEnergyActivity` class and inject `IHttpClientFactory`.
- Add a private readonly field of type `HttpClient` to the class.
- Set this field in the constructor by using the `CreateClient()` method of the `IHttpClientFactory`.

```csharp
private readonly HttpClient client;

public EstimateKineticEnergyActivity(IHttpClientFactoryhttpClientFactory)
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

#### 1.3 Calling the activity from the orchestration

The syntax for calling an activity is:

#### 1.4 Dealing with failure

The syntax for calling an activity with retries is:

### 2. Retrieving the impact probability

Repeat the same substeps as in Step 1 but now for the `https://demo-neo.azure-api.net/neo/estimate/probability` endpoint.

### 3. Retrieving the Torino impact

Repeat the same substeps as in Step 1 but now for the `https://demo-neo.azure-api.net/neo/estimate/torino` endpoint.


### 4. Build en run locally

