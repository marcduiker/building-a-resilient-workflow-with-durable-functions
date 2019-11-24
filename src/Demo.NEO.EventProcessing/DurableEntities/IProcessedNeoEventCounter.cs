using System.Threading.Tasks;

namespace Demo.NEO.EventProcessing.DurableEntities
{
    public interface IProcessedNeoEventCounter
    {
        void Add();
        Task<int> GetAsync();
        void Reset();
    }
}