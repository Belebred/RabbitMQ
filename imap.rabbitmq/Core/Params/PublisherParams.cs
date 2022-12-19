using RabbitMQ.Client;

namespace imap.consumer.Core.Params
{
    public class PublisherParams
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; } = RabbitMQ.Client.ExchangeType.Direct;
        public bool IsDurable { get; set; }
        public bool IsAutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
        public int RetryCount { get; set; }
    }
}