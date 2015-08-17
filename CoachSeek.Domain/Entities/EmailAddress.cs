using System;
using System.Net.Mail;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class EmailAddress
    {
        public string Email { get; private set; } 

        public EmailAddress(string email)
        {
            if (email == null)
                throw new EmailAddressRequired();

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
            catch (Exception)
            {
                // Catch ArgumentException, FormatException
                throw new EmailAddressFormatInvalid();
            }
        }
    }
}
