using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Domain.Entities
{
    public class BusinessAdmin
    {
        public Guid Id { get; set; }

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Email { get { return EmailAddress.Email; } }
        public string Username { get { return Credential.Username; } }
        public string PasswordHash { get { return Credential.PasswordHash; } }

        private PersonName Person { get; set; }
        private EmailAddress EmailAddress { get; set; }
        private Credential Credential { get; set; }

        public BusinessAdmin(string firstName, string lastName, string email, string password)
        {
            Id = Guid.NewGuid();
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            // Email is also the Username.
            Credential = new Credential(email, password);
        }

        public BusinessAdmin(Guid id, string email, 
                             string firstName, string lastName, 
                             string username, string passwordHash, string passwordSalt)
        {
            Id = id;
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            Credential = new Credential(username, passwordHash, passwordSalt);
        }


        public void Validate(IBusinessRepository repository)
        {
            var admin = repository.GetByAdminEmail(Email);
            if (admin.IsExisting())
                throw new DuplicateBusinessAdmin();
        }
    }
}