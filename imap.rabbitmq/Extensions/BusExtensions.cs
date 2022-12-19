using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.rabbitmq.Extensions
{
    public static class BusExtensions
    {
        public static IServiceCollection UseDataBus(this IServiceCollection services)
        {
            services.AddSingletonPersistentConnectionFactory();
            services.AddSingletonChannelsPoolManager();
            return services;
        }
    }
}
