using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.NEO.EventProcessing
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run(
            [ServiceBusTrigger("neo-events", "NEOSubscription1", Connection = "NEOEventsTopic")]string[] mySbMsgs, ILogger log)
        {
            var neoEvents = mySbMsgs.Select(m => JsonConvert.DeserializeObject<DetectedNeoEvent>(m));

            var count = neoEvents.Count();
        }
    }
}
