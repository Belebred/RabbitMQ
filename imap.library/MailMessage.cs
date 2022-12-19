using MimeKit;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace imap.library
{
    public class MailMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Subject { get; set; }
        public List<string> Recipients { get; set; }
        public List<string> From { get; set; }
        public List<string> ReplyTo { get; set; }
        public List<string> HiddenCopy { get; set; }
        public string Body { get; set; }

        public MailMessage(MimeMessage message)
        {
            Subject = message.Subject;
            Recipients = message.To.Select(t => t.Name).ToList();
            From = message.From.Select(t => t.Name).ToList();
            ReplyTo = message.ReplyTo.Select(t => t.Name).ToList();
            HiddenCopy = message.Cc.Select(t => t.Name).ToList();
            Body = string.IsNullOrEmpty(message.HtmlBody) ? "" : Regex.Replace(message.HtmlBody, "<.*?>", string.Empty);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}