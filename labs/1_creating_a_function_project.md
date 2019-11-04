# Lab 1 - Creating an Azure Function Project

## Goal

The goal of this lab is to create a basic Azure Function App with an HTTP Trigger function and to trigger the function locally using a REST client.

## Steps

### 1. Create a Function App with a HttpTrigger function

With your IDE of choice create a new Azure Function project (suggested name: `XasaOnboarding`) with the following options:
- Azure Functions v2 (.NET Core)
- HTTP Trigger function
- Storage Account (AzureWebJobsStorage): `Storage Emulator`
- Authentication level: `Function`

Depending on the IDE, the default Http Trigger function will look something like this:

```csharp
public static class Function1
{
    [FunctionName("Function1")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;

        return name != null
            ? (ActionResult)new OkObjectResult($"Hello, {name}")
            : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
    }
}
```

Looking at the code, try to answer these questions:

> What defines an Azure Function in C#?

> How can this function be triggered?

Functions do not need to be static methods anymore, so feel free to remove `static` from the method and class definitions.

### 2. Run the Function App locally

Build and run the Function App locally. 

> The Azure Function Runtime should start in a console window and show some diagnostic information. If there are any issues with your function you will see the errors in this console. 

> What is the HTTP endpoint the function can be called at?

### 3. Trigger the HTTP function

With your REST client of choice do a GET request to the local endpoint and provide a value for the `name` query string parameter.

> Does the function return the expected output?

Now place a breakpoint in the function method and execute a GET request again.

> Is the breakpoint hit?

### 4. Create a Copy of the Function

As you've noticed, an Azure Function in C# is a method decorated with the `FunctionName` attribute. This attribute contains the identifier of the function.

- Copy the function method and paste it into the same class.
- Change the method name so it's different from the existing method name.
- Leave the `FunctionName` attribute value as it is (so the value in both functions are the same).
- Now build the project.

> What is the build output? Are there errors?

As a best practice, try to limit each class to contain only one function. This way you can use `nameof(ClassName)` as the `FunctionName` variable:

```csharp
public class EchoFunctionHttpTrigger
{
    [FunctionName(nameof(EchoFunctionHttpTrigger))]
    ...
}
```

If everything works as expected continue with the [next lab](2_adding_a_queue_binding.md) to change this default function into something useful.