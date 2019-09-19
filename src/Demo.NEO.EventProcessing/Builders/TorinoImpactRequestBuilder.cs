using System;
using Demo.Neo.Models;

namespace Demo.NEO.EventProcessing.Builders
{
    public static class TorinoImpactRequestBuilder
    {
        public static TorinoImpactRequest Build(
            Guid id,
            float impactProbability,
            float kineticEnergyInMegatonTnt)
        {
            return new TorinoImpactRequest
            {
                Id = id,
                ImpactProbability = impactProbability,
                KineticEnergyInMegatonTnt = kineticEnergyInMegatonTnt
            };
        }
    }
}