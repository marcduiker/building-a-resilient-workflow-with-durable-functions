using Demos.NEO.Estimator;
using FluentAssertions;
using Xunit;

namespace Demo.NEO.Estimator.UnitTests
{
    public class ProbabilityEstimatorTests
    {
        [Theory]
        [InlineData(0.9, 0.99)]
        [InlineData(5, 0.01)]
        public void EstimateProbability(float distance, float expectedProbabilityResult)
        {
            // Act
            float result = ProbabilityEstimator.CalculateByDistance(distance);

            // Assert
            result.Should().BeApproximately(expectedProbabilityResult, 0.01f);
        }
    }
}