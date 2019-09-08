using System;

namespace Demos.NEO.Estimator
{
    public static class KineticEnergyCalculator
    {
        /// This is a completely made up (and incorrect) calculation for the kinetic energy of a
        /// spherical object. Only used to demo Durable Functions in the context of Near Earth Objects.
        public static float CalculateMegatonTnt(
            float diameterInKm,
            float velocityInKmPerSecond)
        {
            // Assume fixed density of 2000 kg/m3
            const float density = 2000;
            // Spherical volume calculation = V = 4/3 * (PI * r^3);
            var volume = 1.25 * Math.PI * Math.Pow(diameterInKm * 1000 / 2, 3);
            var mass = volume * density;
            // Kinetic energy calculation = E = 1/2 * m * v^2
            var energyInJoule = 0.5 * mass * Math.Pow(velocityInKmPerSecond * 1000, 2);
            
            var joulePerMegatonTnt = 4.2 * Math.Pow(10, 15);
            var energyInMegatonTnt = energyInJoule / joulePerMegatonTnt;
            
            return Convert.ToSingle(energyInMegatonTnt);
        }
    }
}