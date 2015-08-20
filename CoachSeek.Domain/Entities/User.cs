﻿using System;
using AutoMapper;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class User
    {
        public Guid Id { get; protected set; }

        public string UserName
        {
            get { return Credential.Username; }
        }

        // When the user is first registered it will have no associated business.
        public Guid? BusinessId { get; set; }
        public string BusinessName { get; set; } // Debug

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Email { get { return EmailAddress.Email; } }
        public string PasswordHash { get { return Credential.PasswordHash; } }

        protected PersonName Person { get; set; }
        protected EmailAddress EmailAddress { get; set; }
        protected Credential Credential { get; set; }


        protected User()
        { }

        public User(UserData data) 
            : this(data.Id,
                   data.BusinessId,
                   data.BusinessName,
                   data.Email,
                   data.FirstName, 
                   data.LastName, 
                   data.Username,
                   data.PasswordHash)
        { }

        public User(Guid id, Guid? businessId, string businessName,
                    string email, string firstName, string lastName, 
                    string username, string passwordHash)
        {
            Id = id;
            BusinessId = businessId;
            BusinessName = businessName;
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            Credential = new Credential(username, passwordHash);
        }


        public virtual void Save(IUserRepository repository)
        {
            var existingUser = repository.GetByUsername(Email);
            if (!existingUser.IsExisting())
                throw new UserInvalid(Email);
            repository.Save(this);
        }

        public UserData ToData()
        {
            return Mapper.Map<User, UserData>(this);
        }
    }
}
