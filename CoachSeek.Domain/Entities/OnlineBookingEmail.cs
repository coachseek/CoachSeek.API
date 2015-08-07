using System.Collections.Generic;

namespace CoachSeek.Domain.Entities
{
    public class OnlineBookingEmail : Email
    {
        private const string ONLINE_BOOKING_FOOTER = "Online booking powered by Coachseek (http://www.coachseek.com)";

        public OnlineBookingEmail(string sender, string recipient, string subject, string body)
            : base(sender, recipient, subject, body, ONLINE_BOOKING_FOOTER)
        { }

        public OnlineBookingEmail(string sender, IList<string> recipients, string subject, string body)
            : base(sender, recipients, subject, body, ONLINE_BOOKING_FOOTER)
        { }
    }
}
