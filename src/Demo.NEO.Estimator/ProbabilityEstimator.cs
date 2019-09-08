using System;

namespace Demos.NEO.Estimator
{
    public static class ProbabilityEstimator
    {
        /// Smaller distance is higher probability.
        /// Distance is from 0.9 AU to 5AU.
        /// Force range from 0.01 - 0.99.
        public static float CalculateByDistance(float distance)
        {
            return MapToRange(0.9f, 5f, 0.99f, 0.01f, distance);
        }
        
        private static float MapToRange(
            float sourceMin, float sourceMax,
            float destinationMin, float destinationMax,
            float value)
        {
            var scale = Math.Abs((destinationMax - destinationMin)) / (sourceMax - sourceMin);
                
            return destinationMax + (Math.Abs(value - sourceMax) * scale);
        }
    }
}