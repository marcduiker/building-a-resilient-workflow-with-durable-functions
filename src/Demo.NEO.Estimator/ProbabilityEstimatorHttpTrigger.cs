using System;
using System.IO;
using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demos.NEO.Estimator
{
    public static class ProbabilityEstimatorHttpTrigger
    {
        [FunctionName(nameof(ProbabilityEstimatorHttpTrigger))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "estimate/probability")]
            HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var neoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(requestBody);
            
            
            var probability = ProbabilityEstimator.CalculateByDistance(neoEvent.Distance);
            
            return new JsonResult(new { probabilityOfHit = probability });
        }

        
    }
}