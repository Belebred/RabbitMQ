using imap.consumer;
using imap.consumer.Core;
using imap.consumer.Core.Params;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.rabbitmq.Extensions
{
    public static class ConsumerExtensions
    {
        public static IServiceCollection AddConsumer(this IServiceCollection services, string sectionName)
        {
            services.AddOptions<ConsumerParams>()
                .Configure<IServiceProvider>((connectParams, provider) =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();

                    connectParams.QueueName = configuration.GetValue($"RabbitMQ:{sectionName}:QueueName", connectParams.QueueName);
                    connectParams.IsDurable = configuration.GetValue($"RabbitMQ:{sectionName}:Durable", connectParams.IsDurable);
                    connectParams.IsAutoDelete = configuration.GetValue($"RabbitMQ:{sectionName}:AutoDelete", connectParams.IsAutoDelete);
                    connectParams.IsAutoAck = configuration.GetValue($"RabbitMQ:{sectionName}:AutoAck", connectParams.IsAutoAck);
                });

            services.AddSingleton<IConsumer>((provider) =>
            {
                var consumerParams = provider.GetRequiredService<IOptions<ConsumerParams>>().Value;
                var pool = provider.GetRequiredService<IChannelsPool>();

                return new Consumer(pool, consumerParams);
            });
            return services;
        }
    }
}
