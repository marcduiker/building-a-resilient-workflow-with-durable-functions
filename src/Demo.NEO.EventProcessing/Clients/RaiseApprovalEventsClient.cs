using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Demo.NEO.EventProcessing.Clients
{
    public class RaiseApprovalEventsClient
    {
        [FunctionName(nameof(RaiseApprovalEventsClient))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "event/{instanceId}/{isApproved}")] HttpRequest req,
            [DurableClient] IDurableClient client,
            string instanceId,
            bool isApproved,
            ILogger log)
        {
            await client.RaiseEventAsync(instanceId, "ApprovalEvent", isApproved);

            return new AcceptedResult();
        }
    }
}