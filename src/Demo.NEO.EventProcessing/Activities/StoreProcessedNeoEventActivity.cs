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
            var blobPath = $"neo/processed/{processedNeoEvent.Date.ToString("YYYYMMdd")}/{processedNeoEvent.TorinoImpact}/{processedNeoEvent.Id}";
            var dynamicBlobBinding = new BlobAttribute(blobPath: blobPath);
            var dynamicStorageBinding = new StorageAccountAttribute("ProcessedNeoStorage");

            using (var writer = binder.Bind<TextWriter>(dynamicBlobBinding))
            {
                await writer.WriteAsync(JsonConvert.SerializeObject(processedNeoEvent));
            }
        }
    }
}
