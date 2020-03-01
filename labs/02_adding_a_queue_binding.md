# Lab 2 - Changing the HTTP Trigger function

## Goal

The goal is to change the implementation of the HTTP Trigger function so it offers some business value for XASA.

### Use case
The HR and IT departments at XASA used to do lots of manual work when new employees joined the company. Now both departments are joining forces and automating the onboarding process. You'll be responsible for writing a small piece of this process. 

In this lab, we'll rewrite the default HTTP Triggered function to pass in a `NewHire` object with a name and an email. This object will be validated and if it is valid it will be put on a queue. In the next lab, another function will be written, which will pick up this message, performs the registration/subscription to grant access and outputs the result to blob storage.

## Steps

### 1. Create the `NewHire` object

Add a class named `NewHire` to the solution and add the properties and method defined below:

```csharp
public class NewHire
{
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Email);
    }
}
```

### 2. Read the `NewHire` from the Request

Update the existing default HTTP trigger function as follows:

- Rename the class and function name to be `RegisterNewHireHttpTrigger`
- Remove the GET option from the `HttpTrigger` attribute.
- Change the incoming message type to `HttpRequestMessage`.
- Deserialize the request body to an object of type `NewHire`
- Perform the IsValid() method on the `NewHire` object. In case the object is valid then return an `OkObjectResult`. In case the object is not valid return an `BadRequestObjectResult`.

The resulting function should look something like this:

```csharp
[FunctionName(nameof(RegisterNewHireHttpTrigger))]
public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
    ILogger log)
{
    var newHire = await message.Content.ReadAsAsync<NewHire>();
    if (newHire.IsValid())
    {
        return new OkObjectResult($"{newHire.Name} is scheduled for registration.");
    }
    
    return new BadRequestObjectResult($"Please provide values for {nameof(NewHire.Name)} and {nameof(NewHire.Email)}");
}
```

### 3. Build and Run the Function 

Build and start the Function App locally. Now do a POST to the local `http://localhost:7071/api/RegisterNewHireHttpTrigger` endpoint and provide a `name` and `email` in the body:

```http
POST http://localhost:7071/api/RegisterNewHireHttpTrigger
Content-Type: application/json

{
    "name" : "<YOUR NAME>",
    "email" : "<YOUR EMAIL>"
}
```

If the response is as expected continue with the next step.

### 4. Add a Queue Ouput Binding

The first part of this function was to validate the input. Now let's add the functionality to put the validated `NewHire` object to a storage queue so it can be picked up by another function.

Decoupling functions by using queues can result in a reliable and scalable architecture. If a function is not available, the queue acts a temporary buffer. Once the function is available again all messages are pulled from the queue and no data is lost.

- Add the following NuGet package to the solution: `Microsoft.Azure.WebJobs.Extensions.Storage`. This package contains the neccesary types for the input and output bindings we'll use.
- Add a `Queue` attribute to the function method and specify the `queueName` and `Connection` parameters:

```csharp
 public async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
    [Queue("xasa-newhire-queue", Connection = "AzureWebJobsStorage")] IAsyncCollector<NewHire> queue,
    ILogger log)
```

> The first parameter in the `Queue` attribute is the name of the queue the result will be put on. If the queue does not exist it will be created.

> The `Connection` parameter contains the name of the storage connection used. This connection should be specified in the `local.settings.json` file when running the Function App locally. When the `AzureWebJobsStorage` setting is set to `UseDevelopmentStorage=true` the local storage emulator will be used (when using a Windows machine). When deployed to Azure this connection should be present as an application setting and should contain the actual connectionstring to an Azure Storage Account.

> Note the `IAsyncCollector<T>` type after the `Queue` attribute. This is a generic type that is used for many different output bindings.

- Update the body of the function to add the `NewHire` object to the queue when it is valid by using the `AddAsync()` method:

```csharp
if (newHire.IsValid())
{
    await queue.AddAsync(newHire);
    return new OkObjectResult($"{newHire.Name} is scheduled for registration.");
}
```

### 5. Build and Run the Function

- Ensure that the Azure Storage Emulator is running (or that we're using an Azure storage account).
- Build and start the Function App locally.
- Repeat the POST to the local endpoint as we did in Step 3.

> Use the Azure Storage Explorer and locate the `xasa-newhire-queue`. Is there a message in this queue?

If everything works as expected, continue with the [next lab](03_create_queuetrigger_function.md) to add a QueueTrigger function to the project.



