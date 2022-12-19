using RabbitMQ.Client;

namespace imap.consumer.Core
{
    public interface IPersistentConnectionFactory
    {
        IConnection TryGetConnection(string postfix);
    }
}