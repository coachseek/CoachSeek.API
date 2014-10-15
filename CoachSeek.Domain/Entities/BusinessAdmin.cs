using CoachSeek.Data.Model;
using System;

namespace CoachSeek.Domain.Entities
{
    public class BusinessAdmin
    {
        public Guid Id { get; protected set; }
        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Email { get { return EmailAddress.Email; } }
        public string Username { get { return Credential.Username; } }
        public string PasswordHash { get { return Credential.PasswordHash; } }
        public string PasswordSalt { get { return Credential.PasswordSalt; } }

        protected PersonName Person { get; set; }
        protected EmailAddress EmailAddress { get; set; }
        protected Credential Credential { get; set; }


        protected BusinessAdmin()
        { }

        public BusinessAdmin(BusinessAdminData data) 
            : this(data.Id, 
                   data.Email, 
                   data.FirstName, 
                   data.LastName, 
                   data.Username,
                   data.PasswordHash, 
                   data.PasswordSalt)
        { }

        public BusinessAdmin(Guid id, string email, 
                             string firstName, string lastName, 
                             string username, string passwordHash, string passwordSalt)
        {
            Id = id;
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            Credential = new Credential(username, passwordHash, passwordSalt);
        }


        public BusinessAdminData ToData()
        {
            return new BusinessAdminData
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Username = Username,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt
            };
        }
    }
}