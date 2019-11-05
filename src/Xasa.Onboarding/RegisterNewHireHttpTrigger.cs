using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Xasa.Onboarding
{
    public class RegisterNewHireHttpTrigger
    {
        [FunctionName(nameof(RegisterNewHireHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("xasa-newhire-queue", Connection = "AzureWebJobsStorage")] IAsyncCollector<NewHire> queue,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newHire = JsonConvert.DeserializeObject<NewHire>(requestBody);
            if (newHire.IsValid())
            {
                await queue.AddAsync(newHire);
                return new OkObjectResult($"{newHire.Name} is scheduled for registration.");
            }
            
            return new BadRequestObjectResult($"Please provide values for {nameof(NewHire.Name)} and {nameof(NewHire.Email)}");
        }
    }
}
