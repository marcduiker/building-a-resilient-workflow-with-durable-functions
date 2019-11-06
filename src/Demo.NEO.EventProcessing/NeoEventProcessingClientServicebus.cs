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
            [ServiceBusTrigger("neo-events", "marc-duiker-30ae5962-40e1-4c9f-b5fd-9cc834edd2f4", Connection = "NEOEventsTopic")]string message, 
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
