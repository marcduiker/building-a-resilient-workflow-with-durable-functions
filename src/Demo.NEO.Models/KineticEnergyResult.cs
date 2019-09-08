using System;
using Newtonsoft.Json;

namespace Demo.Neo.Models
{
    public class KineticEnergyResult
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("kinetic_energy_megaton_tnt")]
        public float KineticEnergyInMegatonTnt { get; set; }
    }
}