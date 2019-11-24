# Lab 8 -  Storing the ProcessedNeoEvent

## Goal

The goal of this lab is to implement the activity which stores all `ProcessedNeoEvent` objects with a Torino impact equal or higher than 1 to to blob storage.

## Steps

### 1. Storage extension

Lets start by adding the following NuGet package to the project: `Microsoft.Azure.WebJobs.Extensions.Storage` so we can work with storage resources.

### 2. Adding a new class for the activity

Now add a new class, `StoreProcessedNeoEventActivity`, to the project and add an activity function method to it. It should accept a `ProcessedNeoEvent` as input.

You might know that Azure Functions can work with a convenient Blob output binding such as:

```csharp
[Blob("neo/processed/file.json", Connection = "ProcessedNeoEventStorage")]string blobContent,
```

But notice that by using the above Blob binding the entire path of the blob is specified at design time. We can't set the path or name dynamically at runtime this way. Using [binding expressions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-expressions-patterns) you can often get quite far but not in this case.

Luckily besides the above declarative binding we can use an imperative binding instead. These are set at runtime so we can configure the path based on properties of the ProcessedNeoEvent:

```csharp
[FunctionName(nameof(StoreProcessedNeoEventActivity))]
public async Task Run(
    [ActivityTrigger] ProcessedNeoEvent processedNeoEvent,
    IBinder binder,
    ILogger logger)
{
    var blobPath = $"neo/processed/{processedNeoEvent.DateDetected:yyyyMMdd}/{processedNeoEvent.TorinoImpact}/{processedNeoEvent.Id}.json";
    var dynamicBlobBinding = new BlobAttribute(blobPath: blobPath) { Connection = "ProcessedNeoStorage" };
    using (var writer = await binder.BindAsync<TextWriter>(dynamicBlobBinding))
    {
        await writer.WriteAsync(JsonConvert.SerializeObject(processedNeoEvent, Formatting.Indented));
    }
}
```

> Notice that the method accepts an `IBinder` and that the `BlobAttribute` is defined in the function method itself. Take a moment to inspect how the blobPath is configured and feel free to change the path to something you prefer.

### 2. Calling the activity from the orchestration

Now let's update the `NeoEventProcessingOrchestrator` function to call the activity. Make sure that the activity is only called for `ProcessedNeoEvent` objects with a TorinoImpact of 1 and higher.

### 3. Build & run locally with HttpTrigger

Now run/debug your local Function App by using the [HttpTrigger client function](../http/start_orchestration.http).

You can use the Azure Storage Explorer tool to look into your your local emulated storage.

> Are blobs being created in the path you defined in the activity function?

### 4. Build & run locally with ServicebusTrigger

If the HttpTrigger function works well and stores a `ProcessedNeoEvent` to blob storage you can try to enable the ServicebusTrigger function in the `local.settigns.json`:

```json
"AzureWebJobs.<FUNCTION_NAME>.Disabled": false
```

Now run the solution locally again and check if the blobs arer being saved to storage. You should give the application some time before the blobs will appear.

Continue to the [next lab](9_send_notification.md) to create the activity which sends a notification.
