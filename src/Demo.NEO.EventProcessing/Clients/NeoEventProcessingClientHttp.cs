using System.Net.Http;
using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Demo.NEO.EventProcessing.Clients
{
    public class NeoEventProcessingClientHttp
    {
        [FunctionName(nameof(NeoEventProcessingClientHttp))]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "start")]HttpRequestMessage message,
            [DurableClient]IDurableClient durableClient,
            ILogger log)
        {
            var detectedNeoEvent = await message.Content.ReadAsAsync<DetectedNeoEvent>();
            var instanceId = await durableClient.StartNewAsync(nameof(NeoEventProcessingOrchestrator),
                detectedNeoEvent);

            log.LogInformation($"HTTP started orchestration with ID {instanceId}.");

            return durableClient.CreateCheckStatusResponse(message, instanceId);
        }
    }
}
