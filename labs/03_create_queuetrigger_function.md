# Lab 3 - Add a QueueTrigger Function

## Goal

The goal of this lab is to extend the Function App with a QueueTrigger function which will take the `NewHire` message from the queue, creates a subscription you'll need in follow up labs and saves the result to blob storage.

## Steps

### 1. Adding a QueueTrigger Function

Let's start with adding a QueueTrigger function to the existing Azure Function project.

When creating the function specify the following:

- Class name: `RegisterNewHireQueueTrigger.cs`
- Connection string setting name: `AzureWebJobsStorage`
- Queue name: `xasa-newhire-queue`

You should en up with something like this (after some code clean-up, and using the `NewHire` object):

```csharp
public class RegisterNewHireQueueTrigger
{
    [FunctionName(nameof(RegisterNewHireQueueTrigger))]
    public void Run([QueueTrigger("xasa-newhire-queue", Connection = "AzureWebJobsStorage")]NewHire newHire, 
        ILogger log)
    {
    }
}
```

### 2. Calling an HTTP Endpoint

This function is going to call an HTTP endpoint which takes care of the user registration.

- Add a reference to NuGet package: `Microsoft.Azure.WebJobs.Extensions.Http`
- Update the function class so it looks like the following:

```csharp
public class RegisterNewHireQueueTrigger
{
    private static readonly HttpClient HttpClient = new HttpClient();
    
    [FunctionName(nameof(RegisterNewHireQueueTrigger))]
    public async Task Run(
        [QueueTrigger("xasa-newhire-queue", Connection = "AzureWebJobsStorage")]NewHire newHire, 
        ILogger log)
    {
        var subscriptionUri = "https://demo-neo.azure-api.net/setup/subscription";
        var queryParams = new Dictionary<string, string> {{ "name", newHire.Name }};
        var subscriptionUriWithQueryParams = QueryHelpers.AddQueryString(subscriptionUri, queryParams);
        var result = await HttpClient.PostAsync(subscriptionUriWithQueryParams, null);
        if (result.IsSuccessStatusCode)
        {
            // TODO
        }
    }
}
```
> Notice the private static `HttpClient` variable. This is a basic solution to limit the risk of running into socket exhaustion exceptions due to opening too many connections. In a later Lab we'll use a much better technique that uses the `HttpClientFactory` and dependency injection.

### 3. Add a Blob Output Binding

The result variable will contain some endpoint and subscription key information you'll need in later Labs. Let's store this result in blob storage so you can retrieve it later.

Because the blob output is optional, only when calling the subscription endpoint resulted in a success, ideally we use an IAsyncCollector<T> as we did for the queue output binding. But unfortunately this does not exist for the blob output binding. Instead we need to use something called an imperative output binding. This is a binding which is configured dynamically at runtime inside the function method.

- Update the function body so it looks like the following:

```csharp
public class RegisterNewHireQueueTrigger
{
    private static readonly HttpClient HttpClient = new HttpClient();
    
    [FunctionName(nameof(RegisterNewHireQueueTrigger))]
    public async Task Run(
        [QueueTrigger("xasa-newhire-queue", Connection = "AzureWebJobsStorage")]NewHire newHire, 
        IBinder binder,
        ILogger log)
    {
        var subscriptionUri = "https://demo-neo.azure-api.net/setup/subscription";
        var queryParams = new Dictionary<string, string> {{ "name", newHire.Name }};
        var subscriptionUriWithQueryParams = QueryHelpers.AddQueryString(subscriptionUri, queryParams);
        var result = await HttpClient.PostAsync(subscriptionUriWithQueryParams, null);
        if (result.IsSuccessStatusCode)
        {
            var subscription = await result.Content.ReadAsAsync<JToken>();
            var dynamicBlobBinding = new BlobAttribute(blobPath: "xasa-subscriptions/{rand-guid}.json");
            using (var writer = await binder.BindAsync<TextWriter>(dynamicBlobBinding))
            {
                await writer.WriteAsync(subscription.ToString(Formatting.Indented));
            }
        }
    }
}
```
> Note the use of the `IBinder` type in the method signature instead of a specific binding such as `Queue` or `Blob`.

> Note that a `BlobAttribute` is created inside the function and a `TextWriter` is used to write the file to the blob.

> Note the usage of `{rand-guid}` as part of the blobPath. What do you think it does? This is called a binding expression. Read more about them [here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-expressions-patterns).

### 4. Build and Run the Function

- Ensure that the Azure Storage Emulator is running (or that you're using an Azure storage account).
- Build and start the Function App locally.
- If you had placed a message on the `xasa-newhire-queue` in the previous lab, you don't need to do a new POST to the HTTP endpoint. The QueueTrigger should detect that a message is in that queue and should be processed.

> Is there a blob available in the `xasa-subscriptions` container?

- Open the json file and inspect the contents. It should contain the following fields:

```json
{
  "serviceBusTopicName": "neo-events",
  "serviceBusConnectionString": "<SERVICEBUS_ENDPOINT>",
  "servicebusTopicSubscriptionName": "<PERSONAL_TOPIC_SUBSCRIPTION",
  "apiManagementKey": "<API_KEY>"
}
```

Great work! You've finished the first Azure Function app!

Now, we'll have a brief recap of labs 1-3 and some theory on Durable Functions. After this, you can continue with the [next lab](04_adding_servicebus_trigger.md) to create a new Azure Function app which will listen to the servicebus.
