using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.consumer.Core.Params
{
    public class ConsumerParams
    {
        public string QueueName { get; set; } = "default-queue";
        public bool IsAutoDelete { get; set; }
        public bool IsDurable { get; set; }
        public bool IsAutoAck { get; set; }
        public bool Exclusive { get; set; }
    }
}
