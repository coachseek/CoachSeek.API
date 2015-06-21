using System;

namespace CoachSeek.Domain.Entities
{
    public class Merchant
    {
        private EmailAddress EmailAddress { get; set; }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get { return EmailAddress.Email; }}

        public Merchant(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            EmailAddress = new EmailAddress(email);
        }
    }
}
