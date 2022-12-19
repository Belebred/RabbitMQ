using RabbitMQ.Client;

namespace imap.consumer
{
    public interface IConsumer
    {
        IModel AddDirectConsumer();
    }
}