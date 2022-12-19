using imap.consumer.Core;
using imap.consumer.Core.Params;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.consumer
{
    public class Publisher : IPublisher
    {
        private readonly ILogger<Publisher> _logger;
        private IChannelsPool _channelsPool;
        private PublisherParams _publisherParams;

        public Publisher(IChannelsPool channelsPool, PublisherParams publisherParams, ILogger<Publisher> logger)
        {
            _logger = logger;
            _publisherParams = publisherParams;
            _channelsPool = channelsPool;
        }

        public void Publish(string message, string queueName, string routingKey)
        {
            InnerPublish(message, queueName, routingKey);
        }

        private void InnerPublish(string message, string queueName, string routingKey)
        {
            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(
                _publisherParams.RetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time, cnt, rCnt) =>
                {
                    _logger.LogWarning("Attempt No: {Attempt} of {MaxAttempt} to publish event to data bus with label {RoutingKey} to exchanger {ExchangeName}",
                        cnt,
                        _publisherParams.RetryCount,
                        routingKey,
                        _publisherParams.ExchangeName);
                }
            );

            var channel = _channelsPool.GetOrAddChannelForPublisher(_publisherParams, routingKey);

            lock (channel)
            {
                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;

                policy.Execute(() =>
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        channel.QueueDeclare(queueName, _publisherParams.IsDurable, false, _publisherParams.IsAutoDelete, null);

                        channel.QueueBind(queueName, _publisherParams.ExchangeName, routingKey);

                        channel.BasicPublish(
                            _publisherParams.ExchangeName,
                            routingKey: routingKey,
                            true,
                            props,
                            Encoding.UTF8.GetBytes(message));
                    }
                    finally 
                    {
                        sw.Stop();

                        if(sw.Elapsed.TotalMilliseconds > 5)
                        {
                            _logger.LogWarning("Timed out {SecondTimeout} when publishing message to the bus", sw.Elapsed.TotalSeconds.ToString());
                        }
                    }
                });
               
            }
        }
    }
}
