using imap.consumer.Core.Params;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Collections.Concurrent;

namespace imap.consumer.Core
{
    public class ChannelsPool : IChannelsPool
    {
        private readonly IPersistentConnectionFactory _connectionFactory;
        private readonly ConcurrentDictionary<string, IModel> _channels;
        private readonly ILogger<ChannelsPool> _logger;
        private IConnection _connectionForPublisher;
        private IConnection _connectionForConsumer;

        public ChannelsPool(IPersistentConnectionFactory connectionFactory, ILogger<ChannelsPool> logger)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _channels = new ConcurrentDictionary<string, IModel>();
        }

        public IModel GetOrAddChannelForConsumer(ConsumerParams consumerParams)
        {
            var key = "";
            var channel = _channels.GetOrAdd(key, (k) =>
            {
                return CreateChannelForConsumer(consumerParams);
            });
            return channel;
        }
        public IModel GetOrAddChannelForPublisher(PublisherParams publisherParams, string routingKey)
        {
            var key = $"publisher-{publisherParams.ExchangeName}-{routingKey}";

            var channel = _channels.GetOrAdd(key, (k) =>
            {
                return CreateChannelForPublisher(publisherParams);
            });
            return channel;
        }

        /// <summary>
        /// Создаёт канал подключения для издателя
        /// </summary>
        /// <returns></returns>
        private IModel CreateChannelForPublisher(PublisherParams publisherParams)
        {
            if (!IsConnected(_connectionForPublisher))
            {
                _connectionForPublisher = _connectionFactory.TryGetConnection("publisher");
            }
            var channel = _connectionForPublisher.CreateModel();

            channel.ExchangeDeclare(publisherParams.ExchangeName,
                                    publisherParams.ExchangeType,
                                    publisherParams.IsDurable,
                                    publisherParams.IsAutoDelete,
                                    publisherParams.Arguments);
            return channel;
        }

        /// <summary>
        /// Создаёт канал подключения для подписчика
        /// </summary>
        /// <returns></returns>
        private IModel CreateChannelForConsumer(ConsumerParams consumerParams)
        {
            if (!IsConnected(_connectionForConsumer))
            {
                _connectionForConsumer = _connectionFactory.TryGetConnection("consumer");
            }

            var channel = _connectionForConsumer.CreateModel();
            
            channel.QueueDeclare(consumerParams.QueueName,
                                 consumerParams.IsDurable,
                                 false,
                                 consumerParams.IsAutoDelete,
                                 null);
            return channel;
        }

        /// <summary>
        /// Проверка существования соединения
        /// </summary>
        /// <param name="connection">Соединение</param>
        /// <returns></returns>
        private bool IsConnected(IConnection connection)
        {
            return connection != null && connection.IsOpen;
        }
    }
}
