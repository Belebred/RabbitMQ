using imap.consumer.Core;
using imap.consumer.Core.Params;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.rabbitmq.Extensions
{
    public static class BusConnectionFactoryExtensions
    {
        public static IServiceCollection AddSingletonPersistentConnectionFactory(this IServiceCollection services)
        {
            services.AddOptions<PersistentConnectionFactoryParams>()
                .Configure<IServiceProvider>((connectParams, provider) =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();

                    connectParams.ClientProvidedName = configuration.GetValue($"RabbitMQ:ClientProvidedName", connectParams.ClientProvidedName);
                    connectParams.HostName = configuration.GetValue($"RabbitMQ:HostName", connectParams.HostName);
                    connectParams.VirtualHost = configuration.GetValue($"RabbitMQ:VirtualHost", connectParams.VirtualHost);
                    connectParams.Port = configuration.GetValue($"RabbitMQ:Port", connectParams.Port);
                    connectParams.UserName = configuration.GetValue($"RabbitMQ:UserName", connectParams.UserName);
                    connectParams.Password = configuration.GetValue($"RabbitMQ:Password", connectParams.Password);
                    connectParams.RetryCount = configuration.GetValue($"RabbitMQ:RetryCount", connectParams.RetryCount);
                    connectParams.ContinuationTimeout = configuration.GetValue($"RabbitMQ:ContinuationTimeout", connectParams.ContinuationTimeout);
                    connectParams.DispatchConsumersAsync = configuration.GetValue($"RabbitMQ:DispatchConsumerAsync", connectParams.DispatchConsumersAsync);
                    connectParams.AutomaticRecoveryEnabled = configuration.GetValue($"RabbitMQ:AutomaticRecoveryEnabled", connectParams.AutomaticRecoveryEnabled);
                    connectParams.NetworkRecoveryInterval = configuration.GetValue($"RabbitMQ:NetworkRecoveryInterval", connectParams.NetworkRecoveryInterval);
                    connectParams.TopologyRecoveryEnabled = configuration.GetValue($"RabbitMQ:TopologyRecoveryEnabled", connectParams.TopologyRecoveryEnabled);
                });

            services.AddSingleton<IPersistentConnectionFactory>((provider) =>
            {
                var logger = provider.GetRequiredService<ILogger<PersistentConnectionFactory>>();
                var connectionFactoryParams = provider.GetRequiredService<IOptions<PersistentConnectionFactoryParams>>().Value;
                var factory = new ConnectionFactory
                {
                    HostName = connectionFactoryParams.HostName,
                    VirtualHost = connectionFactoryParams.VirtualHost,
                    Port = connectionFactoryParams.Port,
                    UserName = connectionFactoryParams.UserName,
                    Password = connectionFactoryParams.Password,
                    RequestedChannelMax = 2047,
                    ContinuationTimeout = TimeSpan.FromSeconds(connectionFactoryParams.ContinuationTimeout),
                    RequestedHeartbeat = TimeSpan.FromSeconds(30),
                    AutomaticRecoveryEnabled = connectionFactoryParams.AutomaticRecoveryEnabled,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(connectionFactoryParams.NetworkRecoveryInterval),
                    TopologyRecoveryEnabled = connectionFactoryParams.TopologyRecoveryEnabled,
                    DispatchConsumersAsync = connectionFactoryParams.DispatchConsumersAsync
                };

                factory.Uri = new Uri(factory.Endpoint.ToString());

                return new PersistentConnectionFactory(factory, connectionFactoryParams.ClientProvidedName);
            });
            return services;
        }
    }
}
