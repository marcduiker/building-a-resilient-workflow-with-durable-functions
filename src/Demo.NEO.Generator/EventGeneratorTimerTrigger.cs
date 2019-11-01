using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Demo.NEO.Generator
{
    public class EventGeneratorTimerTrigger
    {
        [FunctionName(nameof(EventGeneratorTimerTrigger))]
        public async Task Run(
            [TimerTrigger("*/15 * * * * *")] TimerInfo myTimer,
            [ServiceBus("neo-events", Connection = "GeneratorConnection")]IAsyncCollector<DetectedNeoEvent> collector,
            ILogger log)
        {
            var maxNrOfEventsToGenerate = Convert.ToInt32(Environment.GetEnvironmentVariable("MaxNumberOfEventsToGenerate"));
            
            var nrOfEventsToGenerate = new Random().Next(1, maxNrOfEventsToGenerate);
            
            var addToCollectorTasks = NeoEventGenerator.Generate(nrOfEventsToGenerate)
                .Select(neo => collector.AddAsync(neo));
            
            await Task.WhenAll(addToCollectorTasks);
        }
    }
}
