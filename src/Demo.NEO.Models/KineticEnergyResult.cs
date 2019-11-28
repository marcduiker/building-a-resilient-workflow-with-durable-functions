using System;
using Newtonsoft.Json;

namespace Demo.Neo.Models
{
    public class KineticEnergyResult
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("kinetic_energy_megaton_tnt", Required = Required.Always)]
        public float KineticEnergyInMegatonTnt { get; set; }
    }
}