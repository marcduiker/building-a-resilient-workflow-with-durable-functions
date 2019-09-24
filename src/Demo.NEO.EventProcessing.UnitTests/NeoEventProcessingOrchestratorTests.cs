using System.Threading.Tasks;
using Demo.NEO.EventProcessing.UnitTests.TestBuilders;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Demo.NEO.EventProcessing.UnitTests
{
    public class NeoEventProcessingOrchestratorTests
    {
        [Fact]
        public async Task WhenTorinoImpactIsGreaterThan0_ThenStoreProcessedNeoEventActivityShouldBeCalled()
        {
            // Arrange
            var contextMock = DurableOrchestrationContextBaseBuilder.BuildContextWithSpecificTorinoImpactGreaterThan0();
            var logger = new Mock<ILogger>();
            var orchestrator = new NeoEventProcessingOrchestrator();
            
            // Act
            var orchestratorResult = await orchestrator.Run(contextMock.Object, logger.Object);
            
            // Assert
            contextMock.VerifyAll();
        }
        
        [Fact]
        public async Task WhenTorinoImpactIsEqualTo0_ThenStoreProcessedNeoEventActivityShouldNotBeCalled()
        {
            // Arrange
            var contextMock = DurableOrchestrationContextBaseBuilder.BuildContextWithSpecificTorinoImpactEqualTo0();
            var logger = new Mock<ILogger>();
            var orchestrator = new NeoEventProcessingOrchestrator();
            
            // Act
            var orchestratorResult = await orchestrator.Run(contextMock.Object, logger.Object);
            
            // Assert
            contextMock.VerifyAll();
        }

        
    }
}