using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ServiceBus.Fluent.Models;

namespace Demo.NEO.Subscriber
{
    public interface ITopicSubscription
    {
        Task<ImmutableArray<SubscriptionInner>> GetTopicSubscriptionsForName(
            string resourceGroup,
            string servicebusNamespace,
            string servicebusTopic,
            string cleanedName);

        Task<string> CreateTopicSubscription(
            string resourceGroup,
            string serviceBusNamespace,
            string serviceBusTopic,
            string cleanedName);
    }
}