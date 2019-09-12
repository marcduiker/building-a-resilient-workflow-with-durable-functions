using System;
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
    public class KineticEnergyEstimatorHttpTrigger
    {
        [FunctionName(nameof(KineticEnergyEstimatorHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = "estimate/kineticenergy")]
            HttpRequest req, 
            ILogger log)
        {
            // This is here to fake errors with this endpoint so clients need to retry.
            var rnd = new Random().Next(0, 10);
            if (rnd > 8)
            {
                return new ConflictObjectResult( new Exception("Too bad, something went wrong (on purpose, muhahaha! >:P), just try again please!"));
            }

            JsonResult result;
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return new BadRequestErrorMessageResult($"The request does not contain a valid {nameof(DetectedNeoEvent)} object.");
            }

            try
            {
                var neoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(requestBody);
                var kineticEnergyInMegatonTnt = KineticEnergyCalculator.CalculateMegatonTnt(
                    neoEvent.Diameter, 
                    neoEvent.Velocity);
                result = new JsonResult(
                    new KineticEnergyResult
                    {
                        Id = neoEvent.Id, 
                        KineticEnergyInMegatonTnt = kineticEnergyInMegatonTnt
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