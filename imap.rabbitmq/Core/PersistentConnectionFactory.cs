using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace imap.consumer.Core
{
    public class PersistentConnectionFactory : IPersistentConnectionFactory
    {
        private readonly IConnectionFactory _factory;
        private readonly ILogger<PersistentConnectionFactory> _logger;
        private string _clientProvidedName;

        public PersistentConnectionFactory(IConnectionFactory factory, string clientProvidedName)
        {
            _factory = factory;
            //_logger = logger;
            _clientProvidedName = clientProvidedName;
        }

        public IConnection TryGetConnection(string postfix)
        {
            //_logger.LogInformation("Attempt to establish connection");
            Console.WriteLine($"Try to establish connection {_clientProvidedName}.{postfix}");
            var connection = _factory.CreateConnection($"{_clientProvidedName}.{postfix}");
            Console.WriteLine("Establish connection");
            // _logger.LogInformation("Connection created");
            return connection;
        }
    }
}