using System.Threading.Tasks;
using AutoFixture;
using Demo.NEO.EventProcessing.Activities;
using Demo.NEO.EventProcessing.DurableEntities;
using Demo.Neo.Models;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Moq;

namespace Demo.NEO.EventProcessing.UnitTests.TestBuilders
{
    public static class DurableOrchestrationContextBaseBuilder
    {
        public static Mock<IDurableOrchestrationContext> BuildContextWithSpecificTorinoImpactGreaterThan0()
        {
            var fixture = new Fixture();
            var contextMock = new Mock<IDurableOrchestrationContext>(MockBehavior.Strict);
            
            var detectedNeoEvent = fixture.Create<DetectedNeoEvent>();
            contextMock.Setup(ctx => ctx.GetInput<DetectedNeoEvent>())
                .Returns(detectedNeoEvent);

            var kineticEnergyResult = fixture.Build<KineticEnergyResult>()
                .With(k => k.KineticEnergyInMegatonTnt, 1e10f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<KineticEnergyResult>(
                    nameof(EstimateKineticEnergyActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(kineticEnergyResult);
            
            var impactProbabilityResult = fixture.Build<ImpactProbabilityResult>()
                .With(k => k.ImpactProbability, 1f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<ImpactProbabilityResult>(
                    nameof(EstimateImpactProbabilityActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(impactProbabilityResult);

            var torinoImpact = 1;
            var torinoImpactResult = fixture.Build<TorinoImpactResult>()
                .With(p => p.TorinoImpact, torinoImpact)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<TorinoImpactResult>(
                    nameof(EstimateTorinoImpactActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<TorinoImpactRequest>()))
                .ReturnsAsync(torinoImpactResult);

            contextMock.Setup(ctx => ctx.SignalEntity(
                It.IsAny<EntityId>(), 
                "Add",
                null));
            
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<object>(
                    nameof(StoreProcessedNeoEventActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<ProcessedNeoEvent>()))
                .ReturnsAsync(new object());

            return contextMock;
        }
        
         public static Mock<IDurableOrchestrationContext> BuildContextWithSpecificTorinoImpactEqualTo0()
         {
            var fixture = new Fixture();
            var contextMock = new Mock<IDurableOrchestrationContext>(MockBehavior.Strict);
            
            var detectedNeoEvent = fixture.Create<DetectedNeoEvent>();
            contextMock.Setup(ctx => ctx.GetInput<DetectedNeoEvent>())
                .Returns(detectedNeoEvent);

            var kineticEnergyResult = fixture.Build<KineticEnergyResult>()
                .With(k => k.KineticEnergyInMegatonTnt, 1e10f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<KineticEnergyResult>(
                    nameof(EstimateKineticEnergyActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(kineticEnergyResult);
            
            var impactProbabilityResult = fixture.Build<ImpactProbabilityResult>()
                .With(k => k.ImpactProbability, 1f)
                .Create();
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<ImpactProbabilityResult>(
                    nameof(EstimateImpactProbabilityActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<DetectedNeoEvent>()))
                .ReturnsAsync(impactProbabilityResult);
            
            var torinoImpact = 0;
            var torinoImpactResult = fixture.Build<TorinoImpactResult>()
                .With(p => p.TorinoImpact, torinoImpact)
                .Create();
            
            contextMock.Setup(ctx => ctx.CallActivityWithRetryAsync<TorinoImpactResult>(
                    nameof(EstimateTorinoImpactActivity),
                    It.IsAny<RetryOptions>(),
                    It.IsAny<TorinoImpactRequest>()))
                .ReturnsAsync(torinoImpactResult);
            
            contextMock.Setup(ctx => ctx.SignalEntity(
                It.IsAny<EntityId>(), 
                "Add",
                null));

            return contextMock;
        }
    }
}