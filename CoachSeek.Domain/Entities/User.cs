using System;
using AutoMapper;
using CoachSeek.Data.Model;
using Microsoft.AspNet.Identity;

namespace CoachSeek.Domain.Entities
{
    public class User : IUser<string>
    {
        // Note: The ASP.NET Identity framework forces us to use string for Id where we would rather have Guid.
        public string Id { get; protected set; }
        public string UserName
        {
            get { return Credential.Username; }
            set
            {
                // Immutable object so cannot set UserName even though it is enforced by the ASP.NET identity IUser interface.
                throw new NotImplementedException();
            }
        }

        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; } // Debug

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Email { get { return EmailAddress.Email; } }

        public string Password { get { return Credential.Password; } }

        //public string PasswordHash { get { return Credential.PasswordHash; } }
        //public string PasswordSalt { get { return Credential.PasswordSalt; } }

        protected PersonName Person { get; set; }
        protected EmailAddress EmailAddress { get; set; }
        protected Credential Credential { get; set; }


        protected User()
        { }

        public User(UserData data) 
            : this(data.Id, 
                   data.Email, 
                   data.FirstName, 
                   data.LastName, 
                   data.Username,
                   data.Password)
        { }

        public User(Guid id, string email, 
                    string firstName, string lastName, 
                    string username, string password)
        {
            Id = id.ToString();
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            Credential = new Credential(username, password);
        }


        public UserData ToData()
        {
            return Mapper.Map<User, UserData>(this);
        }
    }
}
