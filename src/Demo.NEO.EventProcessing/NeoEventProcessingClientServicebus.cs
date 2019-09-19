using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.NEO.EventProcessing
{
    public class NeoEventProcessingClientServicebus
    {
        [FunctionName(nameof(NeoEventProcessingClientServicebus))]
        public async Task Run(
            [ServiceBusTrigger("neo-events", "NEOSubscription1", Connection = "NEOEventsTopic")]string message, 
            [OrchestrationClient]DurableOrchestrationClientBase orchestrationClient,
            ILogger log)
        {
            var detectedNeoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(message);
            var instanceId = await orchestrationClient.StartNewAsync(nameof(NeoEventProcessingOrchestrator), 
                detectedNeoEvent);

            log.LogInformation($"Servicebus started orchestration with ID {instanceId}.");
        }        
    }
}
