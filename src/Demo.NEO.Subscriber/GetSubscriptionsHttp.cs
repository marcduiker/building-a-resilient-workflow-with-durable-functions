using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent.Models;
using System.Linq;

namespace Demo.NEO.Subscriber
{
    public class GetSubscriptionsHttp
    {
        private IServiceBusManager _serviceBusManager;

        public GetSubscriptionsHttp(IServiceBusManager serviceBusManager)
        {
            _serviceBusManager = serviceBusManager;
        }

        [FunctionName(nameof(GetSubscriptionsHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", "POST", Route = "subscription")] HttpRequest req,
            ILogger log)
        {
            string name = req.Query["name"];
            var cleanedName = name.Trim(' ').Replace(' ', '-').ToLowerInvariant();

            if (string.IsNullOrEmpty(cleanedName))
            {
                return new BadRequestObjectResult("Please provide your name on the query string (?name=YOURNAME)");
            }
                   
            var resourceGroup = Environment.GetEnvironmentVariable("Servicebus_ResourceGroup");
            var servicebusNamespace = Environment.GetEnvironmentVariable("Servicebus_Namespace");
            var servicebusTopic = Environment.GetEnvironmentVariable("Servicebus_Topic");

            ActionResult result = null;
            try
            {
                if (req.Method == HttpMethods.Get)
                {
                    var subs = await _serviceBusManager.Inner.Subscriptions.ListByTopicAsync(
                    resourceGroup,
                    servicebusNamespace,
                    servicebusTopic);
                    var subList = subs.AsEnumerable();
                    result = new OkObjectResult(subList.Where(sub => sub.Name.StartsWith(cleanedName)));
                }
                else if (req.Method == HttpMethods.Post)
                {
                    result = await CreateTopicSubscription(
                        resourceGroup,
                        servicebusNamespace,
                        servicebusTopic,
                        cleanedName);
                }
            }
            catch (Exception ex)
            {
                result = new ConflictObjectResult(ex.Message);
            }

            return result;
        }

        private async Task<ActionResult> CreateTopicSubscription(
            string resourceGroup,
            string serviceBusNamespace,
            string serviceBusTopic,
            string cleanedName)
        {
            var subscriptionName = $"{cleanedName}-{Guid.NewGuid().ToString("D")}";

            var sub = await _serviceBusManager.Inner.Subscriptions.CreateOrUpdateAsync(
                    resourceGroup,
                    serviceBusNamespace,
                    serviceBusTopic,
                    subscriptionName,
                    new SubscriptionInner(defaultMessageTimeToLive: "00:10:00"));

            var apiManagementSubscriptionKey = Environment.GetEnvironmentVariable("ApiManagement_SubscriptionKey");
            var servicebusConnectionStringListenOnly = Environment.GetEnvironmentVariable("Servicebus_ListenOnlyKey");

            var response = new
            {
                serviceBusTopicName = serviceBusTopic,
                serviceBusConnectionString = servicebusConnectionStringListenOnly,
                servicebusTopicSubscriptionName = sub.Name,
                apiManagementKey = apiManagementSubscriptionKey
            };

            return new OkObjectResult(response);
        }
    }
}

