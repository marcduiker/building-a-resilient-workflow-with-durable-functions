using System;
using Newtonsoft.Json;

namespace Demo.Neo.Models
{
    public class TorinoImpactResult
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("torino_impact")]
        public int TorinoImpact { get; set; }
    }
}