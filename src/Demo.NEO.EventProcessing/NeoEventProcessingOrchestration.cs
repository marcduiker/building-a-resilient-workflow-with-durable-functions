using Demo.Neo.Models;
using Demo.NEO.EventProcessing.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Demo.NEO.EventProcessing
{
    public class NeoEventProcessingOrchestration
    {
        [FunctionName(nameof(NeoEventProcessingOrchestration))]
        public async Task<KineticEnergyResult> Run(
          [OrchestrationTrigger] DurableOrchestrationContextBase context,
          ILogger logger)
        {
            var detectedNeoEvent = context.GetInput<DetectedNeoEvent>();

            var kineticEnergy = await context.CallActivityAsync<KineticEnergyResult>(
                nameof(EstimateKineticEnergyActivity),
                detectedNeoEvent);

            return kineticEnergy;
        }
    }
}
