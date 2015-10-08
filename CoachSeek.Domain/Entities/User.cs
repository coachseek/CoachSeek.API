using System;
using System.Threading.Tasks;
using AutoMapper;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class User : IUser
    {
        public Guid Id { get; protected set; }

        public string Username
        {
            get { return Credential.Username; }
        }

        // When the user is first registered it will have no associated business and no role.
        public Guid? BusinessId { get; set; }
        public string BusinessName { get; set; } // Debug
        public Role? UserRole { get; set; }

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Email { get { return EmailAddress.Email; } }
        public string Phone { get { return PhoneNumber.Phone; } }
        public string PasswordHash { get { return Credential.PasswordHash; } }
        public string Role { get { return UserRole.HasValue ? UserRole.Value.ToString() : null; } }

        protected PersonName Person { get; set; }
        protected EmailAddress EmailAddress { get; set; }
        protected PhoneNumber PhoneNumber { get; set; }
        protected Credential Credential { get; set; }


        protected User()
        { }

        public User(UserData data) 
            : this(data.Id,
                   data.BusinessId,
                   data.BusinessName,
                   data.Role,
                   data.Email,
                   data.Phone,
                   data.FirstName, 
                   data.LastName, 
                   data.Username,
                   data.PasswordHash)
        { }

        public User(Guid id, Guid? businessId, string businessName, string role,
                    string email, string phone, string firstName, string lastName, 
                    string username, string passwordHash)
        {
            Id = id;
            BusinessId = businessId;
            BusinessName = businessName;
            UserRole = SetUserRole(businessId, role);
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            PhoneNumber = new PhoneNumber(phone);
            Credential = new Credential(username, passwordHash);
        }

        private Role? SetUserRole(Guid? businessId, string role)
        {
            // TODO: Remove this fixup once we have set all valid users to role 'BusinessAdmin'. 
            if (businessId.HasValue && string.IsNullOrEmpty(role))
                return Common.Role.BusinessAdmin;

            return role.Parse<Role>();
        }


        public virtual async Task SaveAsync(IUserRepository repository)
        {
            var existingUser = await repository.GetByUsernameAsync(Email);
            if (!existingUser.IsExisting())
                throw new UserInvalid(Email);
            await repository.SaveAsync(this);
        }

        public UserData ToData()
        {
            return Mapper.Map<User, UserData>(this);
        }
    }
}
