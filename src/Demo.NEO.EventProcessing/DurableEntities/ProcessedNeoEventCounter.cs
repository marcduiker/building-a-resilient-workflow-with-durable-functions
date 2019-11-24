using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;

namespace Demo.NEO.EventProcessing.DurableEntities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProcessedNeoEventCounter : IProcessedNeoEventCounter
    {
        [JsonProperty("currentCount")]
        public int CurrentCount { get; set; }

        public void Add() => CurrentCount += 1;

        public Task<int> GetAsync() => Task.FromResult(CurrentCount);

        public void Reset() => CurrentCount = 0;
        
        [FunctionName(nameof(ProcessedNeoEventCounter))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<ProcessedNeoEventCounter>();
    }
}