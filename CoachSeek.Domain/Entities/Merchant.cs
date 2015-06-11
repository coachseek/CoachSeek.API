﻿namespace CoachSeek.Domain.Entities
{
    public class Merchant
    {
        private EmailAddress EmailAddress { get; set; }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get { return EmailAddress.Email; }}

        public Merchant(string id, string name, string email)
        {
            Id = id;
            Name = name;
            EmailAddress = new EmailAddress(email);
        }
    }
}
