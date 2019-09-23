using System;
using System.Text;
using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Demo.NEO.EventProcessing.Activities
{
    public class SendNotificationActivity
    {
        [FunctionName(nameof(SendNotificationActivity))]
        public async Task Run(
            [ActivityTrigger] ProcessedNeoEvent processedNeoEvent,
            [SendGrid(ApiKey = "SendGrid.MailSendKey")]IAsyncCollector<SendGridMessage> messageCollector,
            ILogger logger)
        {
            
            var message = new SendGridMessage();
            message.AddTo("mduiker@xpirit.com");
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
        }
    }
}