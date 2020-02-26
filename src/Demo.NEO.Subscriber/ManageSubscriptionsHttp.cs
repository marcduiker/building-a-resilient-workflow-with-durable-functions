using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Demo.NEO.Subscriber
{
    public class ManageSubscriptionsHttp
    {
        private readonly ITopicSubscription _topicSubscription;

        public ManageSubscriptionsHttp(ITopicSubscription topicSubscription)
        {
            _topicSubscription = topicSubscription;
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
                return ActionResultBuilder.GetBadRequest();
            }

            var resourceGroup = Environment.GetEnvironmentVariable("Servicebus_ResourceGroup");
            var servicebusNamespace = Environment.GetEnvironmentVariable("Servicebus_Namespace");
            var servicebusTopic = Environment.GetEnvironmentVariable("Servicebus_Topic");
            var apiManagementSubscriptionKey = Environment.GetEnvironmentVariable("ApiManagement_SubscriptionKey");
            var servicebusConnectionStringListenOnly = Environment.GetEnvironmentVariable("Servicebus_ListenOnlyKey");

            ActionResult result = null;
            try
            {
                var matchingSubs = await _topicSubscription.GetTopicSubscriptionsForName(
                    resourceGroup,
                    servicebusNamespace,
                    servicebusTopic,
                    cleanedName);
                
                if (req.Method == HttpMethods.Get)
                {
                    result = ActionResultBuilder.GetOkResultWithSubscriptions(
                        servicebusConnectionStringListenOnly,
                        apiManagementSubscriptionKey,
                        servicebusTopic,
                        matchingSubs.Select( s=>s.Name ));
                }
                else if (req.Method == HttpMethods.Post)
                {
                    if (matchingSubs.Any())
                    {
                        result = ActionResultBuilder.GetOkResultWithSubscriptions(
                            servicebusConnectionStringListenOnly,
                            apiManagementSubscriptionKey,
                            servicebusTopic,
                            matchingSubs.Select( s=>s.Name ));
                    }
                    else
                    {
                        var subName = await _topicSubscription.CreateTopicSubscription(
                            resourceGroup,
                            servicebusNamespace,
                            servicebusTopic,
                            cleanedName);

                        return ActionResultBuilder.GetOkResultWithSubscriptions(
                            servicebusConnectionStringListenOnly,
                            apiManagementSubscriptionKey,
                            servicebusTopic,
                            new List<string> {subName});
                    }
                }
            }
            catch (Exception ex)
            {
                result = new ConflictObjectResult(ex.Message);
            }

            return result;
        }
    }
}

