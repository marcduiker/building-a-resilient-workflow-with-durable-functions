using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.NEO.EventProcessing
{
    public class NeoEventProcessingClientServicebus
    {
        [FunctionName(nameof(NeoEventProcessingClientServicebus))]
        public async Task Run(
            [ServiceBusTrigger("neo-events", "marc-duiker-db75252d-47f5-4b02-9911-c1378e44612d", Connection = "NEOEventsTopic")]string message, 
            [DurableClient]IDurableClient orchestrationClient,
            ILogger log)
        {
            var detectedNeoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(message);
            var instanceId = await orchestrationClient.StartNewAsync(nameof(NeoEventProcessingOrchestrator), 
                detectedNeoEvent);

            log.LogInformation($"Servicebus started orchestration with ID {instanceId}.");
        }        
    }
}
