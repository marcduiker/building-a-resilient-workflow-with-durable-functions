using Newtonsoft.Json;
using System;

namespace Demo.Neo.Models
{
    public class ProcessedNeoEvent
    {
        public ProcessedNeoEvent(
            DetectedNeoEvent detectedNeoEvent,
            float kineticEnergyInMegatonTnt,
            float impactProbability,
            int  torinoImpact)
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

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("date_detected")]
        public DateTime DateDetected { get; set; }

        [JsonProperty("diameter")]
        public float Diameter { get; set; }

        [JsonProperty("distance")]
        public float Distance { get; set; }

        [JsonProperty("velocity")]
        public float Velocity { get; set; }

        [JsonProperty("kinetic_energy_megaton_tnt")]
        public float KineticEnergyInMegatonTnt { get; set; }

        [JsonProperty("impact_probability")]
        public float ImpactProbability { get; set; }

        [JsonProperty("torino_impact")]
        public int TorinoImpact { get; set; }
    }
}