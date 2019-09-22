using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent.Models;
using System.Collections.Generic;
using System.Linq;

namespace Demo.NEO.Subscriber
{
    public class GetSubscriptionsHttp
    {
        [FunctionName(nameof(GetSubscriptionsHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET", "POST", Route = "subscription")] HttpRequest req,
            ILogger log)
        {
            string name = req.Query["name"];
            var cleanedName = name.Trim(' ').ToLowerInvariant();
            var subscriptionName = $"{cleanedName}-{Guid.NewGuid().ToString("D")}";

            var clientID = Environment.GetEnvironmentVariable("Azure:ClientID");
            var clientSecret = Environment.GetEnvironmentVariable("Azure:ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("Azure:TenantID");
            var subscriptionId = Environment.GetEnvironmentVariable("Azure:SubscriptionID");
            var resourceGroup = Environment.GetEnvironmentVariable("Servicebus:ResourceGroup");
            var servicebusNamespace = Environment.GetEnvironmentVariable("Servicebus:Namespace");
            var servicebusTopic = Environment.GetEnvironmentVariable("Servicebus:Topic");
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientID, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);
            var serviceBusManager = ServiceBusManager.Authenticate(credentials, subscriptionId);
            ActionResult result = null;
            try
            {
                if (req.Method == HttpMethods.Get)
                {
                    var subs = await serviceBusManager.Inner.Subscriptions.ListByTopicAsync(
                    resourceGroup,
                    servicebusNamespace,
                    servicebusTopic);
                    var subList = subs.AsEnumerable();
                    result = new OkObjectResult(subList.Where(sub => sub.Name.StartsWith(cleanedName)));
                }
                else if (req.Method == HttpMethods.Post)
                {
                    var sub = await serviceBusManager.Inner.Subscriptions.CreateOrUpdateAsync(
                    resourceGroup,
                    servicebusNamespace,
                    servicebusTopic,
                    subscriptionName,
                    new SubscriptionInner(defaultMessageTimeToLive: "00:10:00"));
                    result = new OkObjectResult(sub);
                }
            }
            catch (Exception ex)
            {
                result = new ConflictObjectResult(ex);
            }

            return name != null
                ? result
                : new BadRequestObjectResult("Please pass your name on the query string (?name=YOURNAME)");
        }


    }
}

