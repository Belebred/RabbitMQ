using imap.consumer.Core;
using imap.consumer.Core.Params;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace imap.consumer
{
    public class Consumer : IConsumer
    {
        private readonly IChannelsPool _channelsPool;
        private ConsumerParams _consumerParams;

        public Consumer(IChannelsPool pool, ConsumerParams consumerParams)
        {
            _channelsPool = pool;
            _consumerParams = consumerParams;
        }

        public IModel AddDirectConsumer()
        {
            var channel = _channelsPool.GetOrAddChannelForConsumer(_consumerParams);
            AddConsumerToChannel(channel);
            return channel;
        }
        private void AddConsumerToChannel(IModel channel) 
        {
            var asyncConsumer = new AsyncEventingBasicConsumer(channel);
            var i = 0;
            var dir = Directory.CreateDirectory($"C:\\Users\\belou\\OneDrive\\Рабочий стол\\{_consumerParams.QueueName}");
            asyncConsumer.Received += (sender, args) =>
            {
                i++;
                var body = Encoding.UTF8.GetString(args.Body.ToArray());

                using var stream = File.Create($"{dir.FullName}\\mailMessage{i}.json");
                stream.Write(args.Body.ToArray());

                channel.BasicAck(args.DeliveryTag, false);
                return Task.CompletedTask;
            };

            var consumerTag = channel.BasicConsume(
                _consumerParams.QueueName,
                _consumerParams.IsAutoAck,
                asyncConsumer);
        }
    }
}