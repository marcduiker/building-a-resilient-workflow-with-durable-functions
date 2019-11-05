using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xasa.Onboarding
{
    public class RegisterNewHireQueueTrigger
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        
        [FunctionName(nameof(RegisterNewHireQueueTrigger))]
        public async Task Run(
            [QueueTrigger("xasa-newhire-queue", Connection = "AzureWebJobsStorage")]NewHire newHire, 
            IBinder binder,
            ILogger log)
        {
            var subscriptionUri = "https://demo-neo.azure-api.net/setup/subscription";
            var queryParams = new Dictionary<string, string> {{ "name", newHire.Name }};
            var subscriptionUriWithQueryParams = QueryHelpers.AddQueryString(subscriptionUri, queryParams);
            var result = await HttpClient.PostAsync(subscriptionUriWithQueryParams, null);
            if (result.IsSuccessStatusCode)
            {
                var subscription = await result.Content.ReadAsAsync<JToken>();
                var dynamicBlobBinding = new BlobAttribute(blobPath: "xasa-subscriptions/{rand-guid}.json");
                using (var writer = binder.Bind<TextWriter>(dynamicBlobBinding))
                {
                    writer.Write(subscription.ToString(Formatting.Indented));
                }
            }
        }
    }
}
