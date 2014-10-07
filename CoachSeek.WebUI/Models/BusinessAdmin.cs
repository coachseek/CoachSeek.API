using System;
using CoachSeek.Domain;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Extensions;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.Models
{
    public class BusinessAdmin
    {
        public Guid Id { get; set; }

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Email { get { return EmailAddress.Email; } }

        private PersonName Person { get; set; }
        private EmailAddress EmailAddress { get; set; }

        public string Username { get; private set; }
        public string Password { get; set; }


        public BusinessAdmin(string firstName, string lastName, string email, string password)
        {
            Id = Guid.NewGuid();
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);

            Username = email.Trim().ToLower();
            Password = password;
        }

        public BusinessAdmin(Guid id, string firstName, string lastName, string email, string password) 
            : this(firstName, lastName, email, password)
        {
            Id = id;
        }


        public void Validate(IBusinessRepository repository)
        {
            var admin = repository.GetByAdminEmail(Email);
            if (admin.IsExisting())
                ThrowBusinessAdminDuplicateEmailValiationException();
        }


        private void ThrowBusinessAdminDuplicateEmailValiationException()
        {
            throw new ValidationException((int)ErrorCodes.ErrorBusinessAdminDuplicateEmail,
                                          Resources.ErrorBusinessAdminDuplicateEmail,
                                          FormField.Email);
        }
    }
}