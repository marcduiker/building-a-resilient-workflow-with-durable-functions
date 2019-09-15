using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.NEO.EventProcessing
{
    public class NeoEventProcessingOrchestration
    {
        [FunctionName(nameof(NeoEventProcessingOrchestration))]
        public async Task Run(
          [OrchestrationTrigger] DurableOrchestrationContextBase context,
          ILogger logger)
        {

            // Since the orchestrator code is being replayed many times
            // don't depend on non-deterministic behavior or blocking calls such as:
            // - DateTime.Now (use context.CurrentUtcDateTime instead)
            // - Guid.NewGuid (use context.NewGuid instead)
            // - System.IO
            // - Thread.Sleep/Task.Delay (use context.CreateTimer instead)
            //
            // More info: https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-checkpointing-and-replay#orchestrator-code-constraints

            // TODO
            var detectedNeoEvent = context.GetInput<DetectedNeoEvent>();

            //await context.CallActivityAsync("ActivityName", detectedNeoEvent);
        }
    }
}
