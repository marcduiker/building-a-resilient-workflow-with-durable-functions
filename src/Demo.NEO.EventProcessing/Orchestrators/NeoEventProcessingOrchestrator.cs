using System;
using Demo.Neo.Models;
using Demo.NEO.EventProcessing.Activities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Demo.NEO.EventProcessing.Builders;
using Demo.NEO.EventProcessing.DurableEntities;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Demo.NEO.EventProcessing
{
    public class NeoEventProcessingOrchestrator
    {
        [FunctionName(nameof(NeoEventProcessingOrchestrator))]
        public async Task<ProcessedNeoEvent> Run(
          [OrchestrationTrigger] IDurableOrchestrationContext context,
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
            
            var torinoImpact = await context.CallActivityWithRetryAsync<TorinoImpactResult>(
                nameof(EstimateTorinoImpactActivity),
                GetRetryOptions(),
                torinoImpactRequest);

            var processedNeoEvent = ProcessedNeoEventBuilder.Build(
                detectedNeoEvent,
                impactProbability.ImpactProbability,
                kineticEnergy.KineticEnergyInMegatonTnt,
                torinoImpact.TorinoImpact);

            var proxy = context.CreateEntityProxy<IProcessedNeoEventCounter>(
                EntityIdBuilder.BuildForProcessedNeoEventCounter());
            proxy.Add();

            if (processedNeoEvent.TorinoImpact > 0)
            {
                await context.CallActivityWithRetryAsync(
                    nameof(StoreProcessedNeoEventActivity),
                    GetRetryOptions(),
                    processedNeoEvent);
            }

            if (processedNeoEvent.TorinoImpact > 7)
            {
                var approvalResult = await context.WaitForExternalEvent<bool>(
                    "ApprovalEvent",
                    TimeSpan.FromHours(1),
                    false);

                if (approvalResult)
                {
                    await context.CallActivityWithRetryAsync(
                        nameof(RunActivity),
                        GetRetryOptions(),
                        processedNeoEvent);
                }
            }

            return processedNeoEvent;
        }

        [FunctionName(nameof(RunActivity))]
        public Task<string> RunActivity(
            [ActivityTrigger]string input)
        {
            return Task.FromResult<string>("bla");
        }

        private RetryOptions GetRetryOptions()
        {
            return new RetryOptions(TimeSpan.FromSeconds(5), 5) { BackoffCoefficient = 2 };
        }
    }
}
