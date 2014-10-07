using System;
using System.Net.Mail;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain
{
    public class EmailAddress
    {
        public string Email { get; private set; } 

        public EmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new MissingEmailAddress();

            var emailAddress = email.Trim().ToLower();
            ValidateFormat(emailAddress);
            Email = emailAddress;
        }


        private void ValidateFormat(string emailAddress)
        {
            try
            {
                var mailAddress = new MailAddress(emailAddress);
            }
            catch (FormatException)
            {
                throw new InvalidEmailAddressFormat();
            }
        }
    }
}
