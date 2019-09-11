using Demos.NEO.Estimator;
using FluentAssertions;
using Xunit;

namespace Demo.NEO.Estimator.UnitTests
{
    public class TorinoImpactCalculatorTests
    {
        [Theory]
        [InlineData(10e9, 0.99, 10)]
        [InlineData(10e8, 0.99, 9)]
        [InlineData(10e7, 0.99, 8)]
        [InlineData(10e6, 0.99, 7)]
        [InlineData(10e5, 0.99, 6)]
        [InlineData(10e4, 0.99, 5)]
        [InlineData(10e3, 0.99, 4)]
        [InlineData(10e2, 0.99, 3)]
        [InlineData(10e1, 0.99, 2)]
        [InlineData(10e0, 0.99, 1)]
        [InlineData(10e8, 0.6, 5)]
        [InlineData(10e7, 0.6, 5)]
        [InlineData(10e6, 0.6, 4)]
        [InlineData(10e5, 0.6, 4)]
        public void CalculateTorinoImpact(
            float kineticEnegeryInMegatonTnt, 
            float impactProbability, 
            int expectedTorinoResult)
        {
            // Act
            var result = TorinoImpactCalculator.CalculateImpact(kineticEnegeryInMegatonTnt, impactProbability);

            // Assert
            result.Should().Be(expectedTorinoResult);
        }
    }
}