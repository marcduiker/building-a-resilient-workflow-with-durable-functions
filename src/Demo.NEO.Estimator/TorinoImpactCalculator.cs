using System;

namespace Demos.NEO.Estimator
{
    public static class TorinoImpactCalculator
    {
        /// <summary>
        /// Loosely based on https://en.wikipedia.org/wiki/Torino_scale.
        /// This functions returns a fake Torino impact from 3 to 10.
        /// </summary>
        public static int CalculateImpact(
            float kineticEnergyInMegatonTnt,
            float impactProbability)
        {
            float torino = 0f;
            if (impactProbability > 0.9)
            {
                var scale = (10 - 8) / (10e8);
                torino = Convert.ToSingle(Math.Round(8 + kineticEnergyInMegatonTnt * scale));
            }
            else
            {
                var scale = (7 - 3) / (10e8);
                torino = Convert.ToSingle(Math.Round(3 + kineticEnergyInMegatonTnt * scale));
            }

            return Convert.ToInt16(torino);;
        }
    }
}