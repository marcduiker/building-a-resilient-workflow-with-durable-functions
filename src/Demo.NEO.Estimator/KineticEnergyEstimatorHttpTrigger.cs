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
    public static class KineticEnergyEstimatorHttpTrigger
    {
        [FunctionName(nameof(KineticEnergyEstimatorHttpTrigger))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "estimate/kineticenergy")]
            HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var neoEvent = JsonConvert.DeserializeObject<DetectedNeoEvent>(requestBody);
            var kineticEnergyInMegatonTnt = KineticEnergyCalculator.CalculateMegatonTnt(
                neoEvent.Diameter, 
                neoEvent.Velocity);

            return new JsonResult(new { kineticEnergyInMegatonTNT = kineticEnergyInMegatonTnt });
        }
    }
}