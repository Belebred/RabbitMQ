using imap.consumer.Core.Params;
using RabbitMQ.Client;

namespace imap.consumer.Core
{
    public interface IChannelsPool
    {
        IModel GetOrAddChannelForConsumer(ConsumerParams consumerParams);
        IModel GetOrAddChannelForPublisher(PublisherParams publisherParams, string routingKey);
    }
}