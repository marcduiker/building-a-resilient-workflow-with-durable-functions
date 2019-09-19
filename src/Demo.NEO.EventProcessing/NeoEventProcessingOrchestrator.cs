using System;
using Demo.Neo.Models;
using Demo.NEO.EventProcessing.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Demo.NEO.EventProcessing.Builders;

namespace Demo.NEO.EventProcessing
{
    public class NeoEventProcessingOrchestrator
    {
        [FunctionName(nameof(NeoEventProcessingOrchestrator))]
        public async Task<TorinoIimpactResult> Run(
          [OrchestrationTrigger] DurableOrchestrationContextBase context,
          ILogger logger)
        {
            var detectedNeoEvent = context.GetInput<DetectedNeoEvent>();

            var kineticEnergy = await context.CallActivityWithRetryAsync<KineticEnergyResult>(
                nameof(EstimateKineticEnergyActivity),
                GetRetryOptions(),
                detectedNeoEvent);
            
            var impactProbability = await context.CallActivityWithRetryAsync<ImpactProbabilityResult>(
                nameof(EstimateImpactProbabilityActivity),
                GetRetryOptions(),
                detectedNeoEvent);
            
            var torinoImpactRequest = TorinoImpactRequestBuilder.Build(
                detectedNeoEvent.Id,
                impactProbability.ImpactProbability,
                kineticEnergy.KineticEnergyInMegatonTnt);
            
            var torinoImpact = await context.CallActivityWithRetryAsync<TorinoIimpactResult>(
                nameof(EstimateTorinoImpactActivity),
                GetRetryOptions(),
                torinoImpactRequest);

            var processedNeoEvent = ProcessedNeoEventBuilder.Build(
                detectedNeoEvent,
                impactProbability.ImpactProbability,
                kineticEnergy.KineticEnergyInMegatonTnt,
                torinoImpact.TorinoImpact);

            if (processedNeoEvent.TorinoImpact >= 1)
            {
                await context.CallActivityWithRetryAsync(
                    nameof(StoreProcessedNeoEventActivity),
                    GetRetryOptions(),
                    processedNeoEvent);
            }

            return torinoImpact;
        }

        private RetryOptions GetRetryOptions()
        {
            return new RetryOptions(TimeSpan.FromSeconds(5), 5) { BackoffCoefficient = 2 };
        }
    }
}
