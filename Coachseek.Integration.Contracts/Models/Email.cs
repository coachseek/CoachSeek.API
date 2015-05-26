using System.Collections.Generic;

namespace Coachseek.Integration.Contracts.Models
{
    public class Email
    {
        public string Sender { get; set; }
        public IList<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


        public Email(string sender, string recipient, string subject, string body)
        {
            Sender = sender;
            Recipients = new List<string> { recipient };
            Subject = subject;
            Body = body;
        }

        public Email(string sender, IList<string> recipients, string subject, string body)
        {
            Sender = sender;
            Recipients = recipients;
            Subject = subject;
            Body = body;
        }
    }
}
