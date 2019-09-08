using System;
using Newtonsoft.Json;

namespace Demo.Neo.Models
{
    public class ImpactProbabilityResult
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("impact_probability")]
        public float ImpactProbability { get; set; }
    }
}