namespace imap.consumer
{
    public interface IPublisher
    {
        public void Publish(string message,string queueName, string routingKey);
    }
}