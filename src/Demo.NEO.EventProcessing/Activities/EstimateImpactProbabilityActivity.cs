using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Demo.NEO.EventProcessing.Activities
{
    public class EstimateImpactProbabilityActivity
    {
        private readonly HttpClient _client;

        public EstimateImpactProbabilityActivity(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(EstimateImpactProbabilityActivity))]
        public async Task<ImpactProbabilityResult> Run(
          [ActivityTrigger] DetectedNeoEvent neoEvent,
          ILogger logger)
        {
            var impactProbabilityEndpoint = new Uri(Environment.GetEnvironmentVariable("ImpactProbabilityEndpoint"));
            var apiManagementKey = Environment.GetEnvironmentVariable("ApiManagementKey");
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiManagementKey);
            var response = await _client.PostAsJsonAsync(impactProbabilityEndpoint, neoEvent);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(content);
            }
            var result = await response.Content.ReadAsAsync<ImpactProbabilityResult>();

            return result;
        }
    }
}
