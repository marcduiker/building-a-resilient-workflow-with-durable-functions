using System;
using System.Collections.Generic;
using Bogus;
using Bogus.Distributions.Gaussian;
using Demo.Neo.Models;

namespace Demos.Neo.Generator
{
    public class NeoEventGenerator
    {
        public static IEnumerable<DetectedNeoEvent> Generate(int numberOfEvents)
        {
            var diameters = GetGaussianDistributionWithHardLimits(1f, 4f, 0.01f, 10f);
            var distances = GetGaussianDistributionWithHardLimits(2f, 2f, 0.9f, 5f);
            var velocities = GetGaussianDistributionWithHardLimits(15f, 15f, 5f, 30f);
            
            var fakeDetectedNeoEvents = new Faker<DetectedNeoEvent>()
                .RuleFor(e => e.Id, f=> f.Random.Guid())
                .RuleFor(e => e.Date, f => DateTime.UtcNow)
                .RuleFor(e => e.Diameter, f => f.PickRandom(diameters))
                .RuleFor(e => e.Distance, f => f.PickRandom(distances))
                .RuleFor(e => e.Velocity, f => f.PickRandom(velocities));

            var generatedEvents = fakeDetectedNeoEvents.Generate(numberOfEvents);

            return generatedEvents;
        }

        private static IEnumerable<float> GetGaussianDistributionWithHardLimits(float mean, float standardDeviation, float min, float max)
        {
            var faker = new Faker();
            var collection = new List<float>();

            while (collection.Count < 100)
            {
                var randFloat = faker.Random.GaussianFloat(mean, standardDeviation);
                if (randFloat > min && randFloat <= max)
                {
                    collection.Add(randFloat);
                }
            }

            return collection;
        }
    }
}