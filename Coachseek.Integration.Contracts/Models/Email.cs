namespace Coachseek.Integration.Contracts.Models
{
    public class Email
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


        public Email(string sender, string recipient, string subject, string body)
        {
            Sender = sender;
            Recipient = recipient;
            Subject = subject;
            Body = body;
        }
    }
}
