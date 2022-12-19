using imap.consumer.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.rabbitmq.Extensions
{
    public static class ChannelsPoolMangerExtensions
    {
        public static IServiceCollection AddSingletonChannelsPoolManager(this IServiceCollection services)
        {
            services.AddSingleton<IChannelsPool>((provider) =>
            {
                var connection = provider.GetRequiredService<IPersistentConnectionFactory>();
                var logger = provider.GetRequiredService<ILogger<ChannelsPool>>();

                return new ChannelsPool(connection, logger);
            });
            return services;
        }
    }
}
