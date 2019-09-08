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
            PartitionKey = detectedNeoEvent.Date.ToString("YYYYMMDD");
            RowKey = detectedNeoEvent.Id.ToString("D");
            KineticEnergyInMegatonTnt = kineticEnergyInMegatonTnt;
            ImpactProbability = impactProbability;
            TorinoImpact = torinoImpact;
        }
        
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }
        
        public float KineticEnergyInMegatonTnt { get; set; }

        public float ImpactProbability { get; set; }

        public int TorinoImpact { get; set; }
    }
}