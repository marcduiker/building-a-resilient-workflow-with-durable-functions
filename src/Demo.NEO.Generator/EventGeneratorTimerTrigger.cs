using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Demos.Neo.Generator
{
    public class EventGeneratorTimerTrigger
    {
        [FunctionName(nameof(EventGeneratorTimerTrigger))]
        public async Task Run(
            [TimerTrigger("*/10 * * * * *")] TimerInfo myTimer,
            [ServiceBus("neo-events", Connection = "GeneratorConnection")]IAsyncCollector<DetectedNeoEvent> collector,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            var fa = new Faker();
            var nrOfEvents = fa.Random.Number(1, 6);
            var generatedEvents = NeoEventGenerator.Generate(nrOfEvents);
            var tasks = generatedEvents.Select(neoEvent => collector.AddAsync(neoEvent)).ToList();

            await Task.WhenAll(tasks);
        }
    }
}
