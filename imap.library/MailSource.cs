using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.library
{
    public class MailSource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ServiceName { get; set; }
        public string ConnectionString { get; set; }
    }
}
