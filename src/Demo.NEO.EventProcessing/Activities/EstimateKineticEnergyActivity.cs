using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Demo.NEO.EventProcessing.Activities
{
    public class EstimateKineticEnergyActivity
    {
        private readonly HttpClient _client;

        public EstimateKineticEnergyActivity(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(EstimateKineticEnergyActivity))]
        public async Task<KineticEnergyResult> Run(
          [ActivityTrigger] DetectedNeoEvent neoEvent,
          ILogger logger)
        {
            var kineticEnergyEndpoint = new Uri(Environment.GetEnvironmentVariable("KineticEnergyEndpoint"));
            var apiManagementKey = Environment.GetEnvironmentVariable("ApiManagementKey");
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiManagementKey);
            var response = await _client.PostAsJsonAsync(kineticEnergyEndpoint, neoEvent);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new ApplicationException(content);
            }
            var result = await response.Content.ReadAsAsync<KineticEnergyResult>();

            return result;
        }
    }
}
