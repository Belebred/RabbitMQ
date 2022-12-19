using imap.consumer;
using imap.consumer.Core;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Mvc;

namespace imap.service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly consumer.IPublisher _publisher;
        private readonly IConsumer _consumer;
        private readonly IChannelsPool _channelsPool;

        public HomeController(ILogger<HomeController> logger, consumer.IPublisher publisher, IChannelsPool pool, IConsumer consumer)
        {
            _logger = logger;
            _publisher = publisher;
            _channelsPool = pool;
            _consumer = consumer;
        }

        [HttpPost("publish_emails")]
        public IActionResult PublishEmails()
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.mail.ru", 993, true);
                client.Authenticate("belousovmark@mail.ru", "QmKbp9JgXDX62jjBYubq");

                var folders = client.GetFolders(new FolderNamespace('.', ""));
                foreach (var folder in folders)
                {
                    Console.WriteLine(folder.FullName);
                    _publisher.Publish(folder.FullName, "folder", "folderName");
                }
            }

            //start RMQ Exchange
            //for each mailsource start task
            //task creates imap client
            //task get mail messages
            //task publish messages in rmq
            return Ok();
        }

        [HttpPost("consume_emails")]
        public IActionResult ConsumeEmails()
        {
            _consumer.AddDirectConsumer();
            return Ok();
        }
    }
}
