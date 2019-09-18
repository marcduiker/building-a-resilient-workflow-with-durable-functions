using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Demo.NEO.EventProcessing.Activities
{
    public class StoreProcessedNeoEventActivity
    {
        [FunctionName(nameof(StoreProcessedNeoEventActivity))]
        public async Task Run(
          [ActivityTrigger] ProcessedNeoEvent processedNeoEvent,
          IBinder binder,
          ILogger logger)
        {
            var blobPath = $"neo/processed/{processedNeoEvent.DateDetected:yyyyMMdd}/{processedNeoEvent.TorinoImpact}/{processedNeoEvent.Id}.json";
            var dynamicBlobBinding = new BlobAttribute(blobPath: blobPath) { Connection = "ProcessedNeoStorage" };

            using (var writer = await binder.BindAsync<TextWriter>(dynamicBlobBinding))
            {
                await writer.WriteAsync(JsonConvert.SerializeObject(processedNeoEvent, Formatting.Indented));
            }
        }
    }
}
