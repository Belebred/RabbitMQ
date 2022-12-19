using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imap.library
{
    public class ImapMailClient
    {
        private readonly MailKit.Net.Imap.IImapClient _client;
        private readonly string _host, _username, _password;
        private readonly int _port;

        public ImapMailClient(string connectionString)
        {
            var connectionParams = ParseConnectionString(connectionString);
            _username = connectionParams["username"];
            _password = connectionParams["password"];
            _host = connectionParams["host"];
            _port = Convert.ToInt32(connectionParams["port"]);
            _client = new MailKit.Net.Imap.ImapClient();
        }

        public void Reconnect()
        {
            _client.Connect(_host, _port, true);
            _client.Authenticate(_username, _password);
            _client.Inbox.Open(MailKit.FolderAccess.ReadWrite);
        }

        public IEnumerable<string> GetMessages()
        {
            var uids = _client.Inbox.Search(MailKit.Search.SearchQuery.All);
            foreach(var uid in uids)
            {
                var message = _client.Inbox.GetMessage(uid);
                var mailMessage = new MailMessage(message);
                yield return mailMessage.ToString();
            }
        }

        public Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            return connectionString
                .Split(';').Where(kvp => kvp.Contains('='))
                .Select(kvp => kvp.Split(new[] { '=' }, 2))
                .ToDictionary(
                    kvp => kvp[0].Trim(),
                    kvp => kvp[1].Trim(),
                    StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
