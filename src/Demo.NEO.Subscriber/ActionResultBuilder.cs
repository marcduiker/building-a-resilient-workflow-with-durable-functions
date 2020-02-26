using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Demo.NEO.Subscriber
{
    public static class ActionResultBuilder
    {
        public static ActionResult GetOkResultWithSubscriptions(
            string servicebusConnectionStringListenOnly,
            string apiManagementSubscriptionKey,
            string serviceBusTopic, 
            IEnumerable<string> subscriptionNames)
        {
            var response = new
            {
                serviceBusTopicName = serviceBusTopic,
                serviceBusConnectionString = servicebusConnectionStringListenOnly,
                apiManagementKey = apiManagementSubscriptionKey,
                servicebusTopicSubscriptionName = subscriptionNames,
            };

            return new OkObjectResult(response);
        }

        public static ActionResult GetBadRequest()
        {
            return new BadRequestObjectResult("Please provide your name on the query string (?name=YOURNAME)");
        }
    }
}