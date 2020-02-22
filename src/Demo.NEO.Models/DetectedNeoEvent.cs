using System;
using Newtonsoft.Json;

namespace Demo.Neo.Models
{
    public class DetectedNeoEvent
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public DateTime Date { get; set; }

        [JsonProperty("distance", Required = Required.Always)]
        public float Distance { get; set; }

        [JsonProperty("velocity", Required = Required.Always)]
        public float Velocity { get; set; }

        [JsonProperty("diameter", Required = Required.Always)]
        public float Diameter { get; set; }
    }
}
