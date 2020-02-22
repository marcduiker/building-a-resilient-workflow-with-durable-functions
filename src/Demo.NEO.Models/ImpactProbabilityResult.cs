using System;
using Newtonsoft.Json;

namespace Demo.Neo.Models
{
    public class ImpactProbabilityResult
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("impact_probability", Required = Required.Always)]
        public float ImpactProbability { get; set; }
    }
}