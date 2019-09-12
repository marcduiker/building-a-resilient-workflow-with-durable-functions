using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Demo.Neo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demos.NEO.Estimator
{
    public class ProbabilityEstimatorHttpTrigger
    {
        [FunctionName(nameof(ProbabilityEstimatorHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "estimate/impactprobability")]
            HttpRequest req, 
            ILogger log)
        {
            
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return new BadRequestErrorMessageResult($"The request does not contain a valid {nameof(DetectedNeoEvent)} object.");
            }

            JsonResult result;
            try
            {
                var neoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(requestBody);
                var probability = ImpactProbabilityCalculator.CalculateByDistance(neoEvent.Distance);
                result = new JsonResult(
                    new ImpactProbabilityResult
                    {
                        Id = neoEvent.Id,
                        ImpactProbability = probability
                    });
            }
            catch (JsonSerializationException e)
            {
                return new BadRequestErrorMessageResult(e.Message);
            }

            return result;
        }
    }
}