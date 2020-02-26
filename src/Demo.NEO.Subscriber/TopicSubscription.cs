using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent.Models;

namespace Demo.NEO.Subscriber
{
    public class TopicSubscription : ITopicSubscription
    {
        private readonly IServiceBusManager _serviceBusManager;
        
        public TopicSubscription(IServiceBusManager serviceBusManager)
        {
            _serviceBusManager = serviceBusManager;
        }
        
        public async Task<ImmutableArray<SubscriptionInner>> GetTopicSubscriptionsForName(
            string resourceGroup,
            string servicebusNamespace,
            string servicebusTopic,
            string cleanedName)
        {
            var subs = await _serviceBusManager.Inner.Subscriptions.ListByTopicAsync(
                resourceGroup,
                servicebusNamespace,
                servicebusTopic);
            var matchingSubs = subs.Where(sub => sub.Name.StartsWith(cleanedName)).ToImmutableArray();
            
            return matchingSubs;
        }
        
        public async Task<string> CreateTopicSubscription(
            string resourceGroup,
            string serviceBusNamespace,
            string serviceBusTopic,
            string cleanedName)
        {
            const int maxSubscriptionNameLength = 50;
            var subscriptionName = $"{cleanedName}-{Guid.NewGuid():N}";
            if (subscriptionName.Length > maxSubscriptionNameLength)
            {
                subscriptionName = subscriptionName.Substring(0, maxSubscriptionNameLength);
            }

            var sub = await _serviceBusManager.Inner.Subscriptions.CreateOrUpdateAsync(
                resourceGroup,
                serviceBusNamespace,
                serviceBusTopic,
                subscriptionName,
                new SubscriptionInner(defaultMessageTimeToLive: "00:10:00"));

            return sub.Name;
        }
    }
}