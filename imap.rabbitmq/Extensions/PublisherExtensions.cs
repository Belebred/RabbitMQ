using imap.consumer;
using imap.consumer.Core;
using imap.consumer.Core.Params;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.rabbitmq.Extensions
{
    public static class PublisherExtensions
    {
        public static IServiceCollection AddPublisher(this IServiceCollection services, string sectionName)
        {
            services.AddOptions<PublisherParams>()
                .Configure<IServiceProvider>((connectParams, provider) =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();

                    connectParams.ExchangeName = configuration.GetValue($"RabbitMQ:{sectionName}:ExchangeName", connectParams.ExchangeName);
                    connectParams.ExchangeType = configuration.GetValue($"RabbitMQ:{sectionName}:ExchangeType", connectParams.ExchangeType);
                    connectParams.IsDurable = configuration.GetValue($"RabbitMQ:{sectionName}:Durable", connectParams.IsDurable);
                    connectParams.IsAutoDelete = configuration.GetValue($"RabbitMQ:{sectionName}:AutoDelete", connectParams.IsAutoDelete);
                    connectParams.Arguments = configuration.GetValue($"RabbitMQ:{sectionName}:Arguments", connectParams.Arguments);
                    connectParams.RetryCount = configuration.GetValue($"RabbitMQ:{sectionName}:RetryCount", connectParams.RetryCount);
                });
            services.AddSingleton<IPublisher>((provider) =>
            {
                var logger = provider.GetRequiredService<ILogger<Publisher>>();
                var publisherParams = provider.GetRequiredService<IOptions<PublisherParams>>().Value;
                var pool = provider.GetRequiredService<IChannelsPool>();

                return new Publisher(pool, publisherParams);
            });
            return services;
        }
    }
}
