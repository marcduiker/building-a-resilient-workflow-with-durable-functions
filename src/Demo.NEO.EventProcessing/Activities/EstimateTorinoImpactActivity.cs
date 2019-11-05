using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Demo.NEO.EventProcessing.Activities
{
    public class EstimateTorinoImpactActivity
    {
        private readonly HttpClient _client;

        public EstimateTorinoImpactActivity(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(EstimateTorinoImpactActivity))]
        public async Task<TorinoImpactResult> Run(
          [ActivityTrigger] TorinoImpactRequest torinoImpactRequest,
          ILogger logger)
        {
            var torinoImpactEndpoint = new Uri(Environment.GetEnvironmentVariable("TorinoImpactEndpoint"));
            var apiManagementKey = Environment.GetEnvironmentVariable("ApiManagementKey");
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiManagementKey);
            var response = await _client.PostAsJsonAsync(torinoImpactEndpoint, torinoImpactRequest);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(content);
            }
            var result = await response.Content.ReadAsAsync<TorinoImpactResult>();

            return result;
        }
    }
}
