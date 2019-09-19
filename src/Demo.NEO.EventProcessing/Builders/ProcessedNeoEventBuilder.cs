using Demo.Neo.Models;

namespace Demo.NEO.EventProcessing.Builders
{
    public static class ProcessedNeoEventBuilder
    {
        public static ProcessedNeoEvent Build(
            DetectedNeoEvent detectedNeoEvent,
            float impactProbability,
            float kineticEnergyInMegatonTnt,
            int torinoImpact)
        {
            return new ProcessedNeoEvent(
                detectedNeoEvent,
                kineticEnergyInMegatonTnt,
                impactProbability,
                torinoImpact);
        }
    }
}