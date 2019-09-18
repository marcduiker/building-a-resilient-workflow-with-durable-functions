using System;

namespace Demo.Neo.Models
{
    public class ProcessedNeoEvent : DetectedNeoEvent
    {
        public ProcessedNeoEvent(
            DetectedNeoEvent detectedNeoEvent,
            float kineticEnergyInMegatonTnt,
            float impactProbability,
            int  torinoImpact)
        {
            Date = detectedNeoEvent.Date;
            Diameter = detectedNeoEvent.Diameter;
            Distance = detectedNeoEvent.Distance;
            Id = detectedNeoEvent.Id;
            Velocity = detectedNeoEvent.Velocity;
            KineticEnergyInMegatonTnt = kineticEnergyInMegatonTnt;
            ImpactProbability = impactProbability;
            TorinoImpact = torinoImpact;
        }
        
        
        
        public float KineticEnergyInMegatonTnt { get; set; }

        public float ImpactProbability { get; set; }

        public int TorinoImpact { get; set; }
    }
}