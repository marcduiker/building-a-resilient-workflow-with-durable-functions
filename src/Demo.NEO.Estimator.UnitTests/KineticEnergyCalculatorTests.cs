using System;
using Demos.NEO.Estimator;
using FluentAssertions;
using Xunit;

namespace Demo.NEO.Estimator.UnitTests
{
    public class KineticEnergyCalculatorTests
    {
        [Theory]
        [InlineData(10f, 10f, 1.1687e7)]
        [InlineData(0.1f, 10f, 1.1687e1)]
        public void CalculateMegatonTnt(float diameterInKm, float velocityInKmPerSec, float kineticEnergyResult)
        {
            // Act
            var result = KineticEnergyCalculator.CalculateMegatonTnt(diameterInKm, velocityInKmPerSec);

            // Assert
            result.Should().BeApproximately(kineticEnergyResult, 1000);
        }
    }
}