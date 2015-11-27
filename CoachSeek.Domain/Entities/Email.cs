using System.Collections.Generic;
using System.Text;

namespace CoachSeek.Domain.Entities
{
    public class Email
    {
        public string Sender { get; private set; }
        public IList<string> Recipients { get; private set; }
        public string Subject { get;  private set; }
        public string Body { get; private set; }


        public Email(string sender, string recipient, string subject, string body, string footer = null)
        {
            Sender = sender;
            Recipients = new List<string> { recipient };
            Subject = subject;
            Body = AppendFooterToBody(body, footer);
        }

        public Email(string sender, IList<string> recipients, string subject, string body, string footer = null)
        {
            Sender = sender;
            Recipients = recipients;
            Subject = subject;
            Body = AppendFooterToBody(body, footer);
        }


        private string AppendFooterToBody(string body, string footer)
        {
            if (string.IsNullOrEmpty(footer))
                return body;
            var builder = new StringBuilder(body);
            builder.Append(footer);
            return builder.ToString();
        }
    }
}
