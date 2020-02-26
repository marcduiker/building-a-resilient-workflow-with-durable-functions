using Demo.NEO.Subscriber;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Demo.NEO.Subscriber
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var clientID = Environment.GetEnvironmentVariable("Azure_ClientID");
            var clientSecret = Environment.GetEnvironmentVariable("Azure_ClientSecret");
            var tenantId = Environment.GetEnvironmentVariable("Azure_TenantID");
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientID, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);

            var subscriptionId = Environment.GetEnvironmentVariable("Azure_SubscriptionID");
            var serviceBusManager = ServiceBusManager.Authenticate(credentials, subscriptionId);

            builder.Services.AddSingleton((s) => serviceBusManager);
        }
    }
}
