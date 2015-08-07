using System.Collections.Generic;

namespace CoachSeek.Domain.Entities
{
    public class BouncedEmail : Email
    {
        public BouncedEmail(string sender, string recipient, string subject, string body) 
            : base(sender, recipient, subject, body)
        { }

        public BouncedEmail(string sender, IList<string> recipients, string subject, string body, string footer = null)
            : base(sender, recipients, subject, body)
        { }
    }
}
