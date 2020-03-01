# Lab 9 -  Notify Bruce

## Goal

The goal of this lab is to implement the activity which sends an email to Bruce Willis, since he's obviously [the only one who can save the planet](https://en.wikipedia.org/wiki/Armageddon_(1998_film)). 

Make sure we only send out an email for detected NEOs with a Torino impact of 8 or higher. Bruce will not come out of his bed for non-Earth-threatening events.

## Steps

### 1. SendGrid account

In order to send emails we're going to use a service to take care of this. Go to  [signup.sendgrid.com/](https://signup.sendgrid.com/) in order to create a (free) account.

### 2. SendGrid API key

In order to use the SendGrid service an API key is required.

- In the SendGrid Dahboard go to Settings > API keys
- Create a new API key
- Enter a name for it
- Select restricted access and configure full access for only the Mail Send category.
- Click Create & View and copy the key to the `local.settings.json` file of our Function App (e.g. `SendGrid.MailSendKey:<YOUR_SENDGRID_APIKEY>`).

### 3. SendGrid extension

Lets start by adding the following NuGet package to the project: `Microsoft.Azure.WebJobs.Extensions.SendGrid` so we can work with SendGrid resources.

### 4. Adding a new class for the activity

Now add a class (`SendNotificationActivity`) and add an activity function to it. 

- The function should accept an `ProcessedNeoEvent` as the input and should not return anything. 
- Use the `SendGrid` attribute and specify the name of the app setting which contains the SendGrid API key. 
- Use the `IAsyncCollector<SendGridMessage>` as the type used with the `SendGrid` attribute.

The function signature should look something like this:

```csharp
 [FunctionName(nameof(SendNotificationActivity))]
public async Task Run(
        [ActivityTrigger] ProcessedNeoEvent processedNeoEvent,
        [SendGrid(ApiKey = "SendGrid.MailSendKey")]IAsyncCollector<SendGridMessage> messageCollector,
        ILogger logger)
```
Now we can implement the creation of a `SendGridMessage` which will be added to the messageCollector.

You need to specify the following:
- Subject
- From address
- To Address (Unfortunalty I don't have Bruce's email address, so I suggest we use our own email here)
- Content (optional)
- Attachment (optional)

This could be a possible implementation of the message:

```csharp
var message = new SendGridMessage();
message.AddTo("yourname@yourdomain.com");
message.SetFrom("alerts@xasa.com");
message.SetSubject("Please help us!");
var content = "<p>Bruce, our planet in is severe danger!</p>" +
              "<p>You are the only one who can stop a giant asteroid (see attachment). Please nuke it now!</p>" +
              "<p>Best regards, Humanity</p>";
message.AddContent(MimeType.Html, content);
var attachment = Convert.ToBase64String(
    Encoding.UTF8.GetBytes(
        JsonConvert.SerializeObject(processedNeoEvent)));
message.AddAttachment($"{processedNeoEvent.Id}.json", attachment);
await messageCollector.AddAsync(message);
```

### 2. Calling the activity from the orchestration

Now update the orchestrator function and call the activity after the `ProcessedNeoEvent` has been saved to storage. Make sure that the activity in only called for events with a Torino impact of 8 and higher.

### 3. Build & run locally

Now run/debug our local Function App by using the [HttpTrigger client function](../http/start_orchestration.http) so we can specify the input which will result in a Torino impact of 8 or higher. You might want to disable the ServicebusTrigger again in the `local.settings.json`.

Let's watch our inbox (and Junk mail) to see if we're receiving an email. If we don't receive anything, we can use the SendGrid dashboard to see if calls to the SendGrid API were made succesfully.

When we also want to test with the `ServicebusTrigger` note that the generated NEOEvents very rarely result in a Torino impact of 8 or higher. So for quick testing purposes we might want to change the orchestrator code to call the notification activity for events with a Torino impact of 5 and higher instead (or use the HttpTrigger function).

Continue to the [next lab](10_unit_testing.md) to add unit tests for our orchestrator function.
