using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.consumer.Core.Params
{
    public class PersistentConnectionFactoryParams
    {
        /// <summary>
        /// Наименование клиента.
        /// </summary>
        public string ClientProvidedName { get; set; } = "client-provider-name";

        /// <summary>
        /// Основной хост.
        /// </summary>
        public string HostName { get; set; } = "localhost";

        /// <summary>
        /// Виртуальный хост.
        /// </summary>
        /// <remarks>
        /// Используется как логическая группа для группировки: обменников, соединений, 
        /// очередей, привязок, разрешений пользователей и других системных ресурсов.
        /// </remarks>
        public string VirtualHost { get; set; } = ConnectionFactory.DefaultVHost;

        /// <summary>
        /// Порт.
        /// </summary>
        public int Port { get; set; } = AmqpTcpEndpoint.DefaultAmqpSslPort;

        /// <summary>
        /// Наименование пользователя.
        /// </summary>
        public string UserName { get; set; } = ConnectionFactory.DefaultUser;

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; } = ConnectionFactory.DefaultPass;

        /// <summary>
        /// Общее время ожидания для всех операции соединения.
        /// </summary>
        public int ContinuationTimeout { get; set; } = 10;

        /// <summary>
        /// Асинхронный диспетчер потребителей(только с IAsyncBasicConsumer)?
        /// </summary>
        public bool DispatchConsumersAsync { get; set; } = true;

        /// <summary>
        /// Автовосстановление соединений?
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; } = true;

        /// <summary>
        /// Пауза перед автовосстановлением соединения.
        /// </summary>
        public int NetworkRecoveryInterval { get; set; } = 10;

        /// <summary>
        /// Автовосстановление обменов, очередей, привязок и потребителей?
        /// </summary>
        public bool TopologyRecoveryEnabled { get; set; } = true;

        /// <summary>
        /// Количество повторных попыток подключения.
        /// </summary>
        public int RetryCount { get; set; } = 5;
    }
}
