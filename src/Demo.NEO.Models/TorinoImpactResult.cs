using System;
using Newtonsoft.Json;

namespace Demo.Neo.Models
{
    public class TorinoImpactResult
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("torino_impact", Required = Required.Always)]
        public int TorinoImpact { get; set; }
    }
}