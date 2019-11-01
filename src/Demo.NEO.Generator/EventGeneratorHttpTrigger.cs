using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Demo.Neo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

namespace Demo.NEO.Generator
{
    public class EventGeneratorHttpTrigger
    {
        [FunctionName(nameof(EventGeneratorHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger("GET", Route = "generate/neoevents/{numberOfEvents}")] HttpRequestMessage request,
            [ServiceBus("neo-events", Connection = "GeneratorConnection")]IAsyncCollector<DetectedNeoEvent> collector,
            int numberOfEvents)
        {
            var generatedEvents = NeoEventGenerator.Generate(numberOfEvents);

            var addToCollectorTasks = generatedEvents.Select(neo => collector.AddAsync(neo));

            await Task.WhenAll(addToCollectorTasks);

            return new JsonResult(generatedEvents);
        }
    }
}
