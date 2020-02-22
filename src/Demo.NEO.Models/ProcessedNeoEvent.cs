using Newtonsoft.Json;
using System;

namespace Demo.Neo.Models
{
    public class ProcessedNeoEvent
    {
        public ProcessedNeoEvent()
        {}
        
        public ProcessedNeoEvent(
            DetectedNeoEvent detectedNeoEvent,
            float kineticEnergyInMegatonTnt,
            float impactProbability,
            int torinoImpact)
        {
            DateDetected = detectedNeoEvent.Date;
            Diameter = detectedNeoEvent.Diameter;
            Distance = detectedNeoEvent.Distance;
            Id = detectedNeoEvent.Id;
            Velocity = detectedNeoEvent.Velocity;
            KineticEnergyInMegatonTnt = kineticEnergyInMegatonTnt;
            ImpactProbability = impactProbability;
            TorinoImpact = torinoImpact;
        }

        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("date_detected", Required = Required.Always)]
        public DateTime DateDetected { get; set; }

        [JsonProperty("diameter", Required = Required.Always)]
        public float Diameter { get; set; }

        [JsonProperty("distance", Required = Required.Always)]
        public float Distance { get; set; }

        [JsonProperty("velocity", Required = Required.Always)]
        public float Velocity { get; set; }

        [JsonProperty("kinetic_energy_megaton_tnt", Required = Required.Always)]
        public float KineticEnergyInMegatonTnt { get; set; }

        [JsonProperty("impact_probability", Required = Required.Always)]
        public float ImpactProbability { get; set; }

        [JsonProperty("torino_impact", Required = Required.Always)]
        public int TorinoImpact { get; set; }
    }
}