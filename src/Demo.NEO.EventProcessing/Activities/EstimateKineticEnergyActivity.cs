using Demo.Neo.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.NEO.EventProcessing.Activities
{
    public class EstimateKineticEnergyActivity
    {
        private readonly HttpClient client;

        public EstimateKineticEnergyActivity(IHttpClientFactory httpClientFactory)
        {
            client = httpClientFactory.CreateClient();
        }

        [FunctionName(nameof(EstimateKineticEnergyActivity))]
        public async Task<KineticEnergyResult> Run(
          [ActivityTrigger] DetectedNeoEvent neoEvent,
          ILogger logger)
        {
            var kineticEnergyEndpoint = new Uri(Environment.GetEnvironmentVariable("KineticEnergyEndpoint"));
            var apiManagementKey = Environment.GetEnvironmentVariable("ApiManagementKey");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiManagementKey);
            var response = await client.PostAsJsonAsync(kineticEnergyEndpoint, neoEvent);
            var result = await response.Content.ReadAsAsync<KineticEnergyResult>();

            return result;
        }
    }
}
