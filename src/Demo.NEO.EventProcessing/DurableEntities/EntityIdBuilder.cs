using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Demo.NEO.EventProcessing.DurableEntities
{
    public static class EntityIdBuilder
    {
        public static EntityId BuildForProcessedNeoEventCounter()
        {
            return new EntityId(nameof(ProcessedNeoEventCounter), "counter");
        }
    }
}