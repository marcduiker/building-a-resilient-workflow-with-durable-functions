using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Xasa.Onboarding
{
    public class RegisterNewHireHttpTrigger
    {
        [FunctionName(nameof(RegisterNewHireHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
            [Queue("xasa-newhire-queue", Connection = "AzureWebJobsStorage")] IAsyncCollector<NewHire> queue,
            ILogger log)
        {
            var newHire = await message.Content.ReadAsAsync<NewHire>();
            if (newHire.IsValid())
            {
                await queue.AddAsync(newHire);
                return new OkObjectResult($"{newHire.Name} is scheduled for registration.");
            }
            
            return new BadRequestObjectResult($"Please provide values for {nameof(NewHire.Name)} and {nameof(NewHire.Email)}");
        }
    }
}
