using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Demo.NEO.EventProcessing.DurableEntities
{
    public static class ProcessedNeoEventCounterHttp
    {
        [FunctionName(nameof(ProcessedNeoEventCounterHttp))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req,
            [DurableClient] IDurableClient durableClient,
            ILogger log)
        {
            var neoEventCountEntity = await durableClient.ReadEntityStateAsync<ProcessedNeoEventCounter>(
                EntityIdBuilder.BuildForProcessedNeoEventCounter());
            if (neoEventCountEntity.EntityExists)
            {
                var count = await neoEventCountEntity.EntityState.GetAsync();
                return new OkObjectResult($"{count} NEO events have been processed.");
            }
            
            return new NotFoundObjectResult("The IProcessedNeoEventCounter entity was not found.");
        }
    }
}