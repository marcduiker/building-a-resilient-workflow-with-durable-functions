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
    public class TorinoEstimatorHttpTrigger
    {
        [FunctionName(nameof(TorinoEstimatorHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "estimate/torinoimpact")]
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
                var torinoImpactRequest = JsonConvert.DeserializeObject<TorinoImpactRequest>(requestBody);
                var torinoImpact = TorinoImpactCalculator.CalculateImpact(
                    torinoImpactRequest.KineticEnergyInMegatonTnt, 
                    torinoImpactRequest.ImpactProbability);
                
                result = new JsonResult(new TorinoIimpactResult
                {
                    Id = torinoImpactRequest.Id,
                    TorinoImpact = torinoImpact
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