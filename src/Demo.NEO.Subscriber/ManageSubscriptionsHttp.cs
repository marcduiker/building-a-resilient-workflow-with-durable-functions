using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent.Models;
using System.Linq;

namespace Demo.NEO.Subscriber
{
    public class ManageSubscriptionsHttp
    {
        private IServiceBusManager _serviceBusManager;

        public ManageSubscriptionsHttp(IServiceBusManager serviceBusManager)
        {
            _serviceBusManager = serviceBusManager;
        }

        [FunctionName(nameof(ManageSubscriptionsHttp))]
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
                var matchingSubs = await GetTopicSubscriptionsForName(
                    resourceGroup,
                    servicebusNamespace,
                    servicebusTopic,
                    cleanedName);
                
                if (req.Method == HttpMethods.Get)
                {
                    result = new OkObjectResult(matchingSubs);
                }
                else if (req.Method == HttpMethods.Post)
                {
                    
                    if (matchingSubs.Any())
                    {
                        result = new OkObjectResult(matchingSubs);
                    }
                    else
                    {
                        result = await CreateTopicSubscription(
                            resourceGroup,
                            servicebusNamespace,
                            servicebusTopic,
                            cleanedName);
                    }
                }
            }
            catch (Exception ex)
            {
                result = new ConflictObjectResult(ex.Message);
            }

            return result;
        }

        private async Task<IEnumerable<SubscriptionInner>> GetTopicSubscriptionsForName(
            string resourceGroup,
            string servicebusNamespace,
            string servicebusTopic,
            string cleanedName)
        {
            var subs = await _serviceBusManager.Inner.Subscriptions.ListByTopicAsync(
                resourceGroup,
                servicebusNamespace,
                servicebusTopic);
            var matchingSubs = subs.Where(sub => sub.Name.StartsWith(cleanedName));
            
            return matchingSubs;
        }

        private async Task<ActionResult> CreateTopicSubscription(
            string resourceGroup,
            string serviceBusNamespace,
            string serviceBusTopic,
            string cleanedName)
        {
            const int maxSubscriptionNameLength = 50;
            var subscriptionName = $"{cleanedName}-{Guid.NewGuid():N}";
            if (subscriptionName.Length > maxSubscriptionNameLength)
            {
                subscriptionName = subscriptionName.Substring(0, maxSubscriptionNameLength);
            }

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

