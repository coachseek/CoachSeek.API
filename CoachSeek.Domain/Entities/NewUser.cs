﻿using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class NewUser : User
    {
        public NewUser(UserAddCommand command)
            : this(command.FirstName, command.LastName, command.Email, command.Phone, command.Password)
        { }

        private NewUser(string firstName, string lastName, string email, string phone, string password)
        {
            Id = Guid.NewGuid();
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            // Email is also the Username.
            PhoneNumber = new PhoneNumber(phone);
            Credential = new NewCredential(email, password);
        }


        public override void Save(IUserRepository repository)
        {
            var user = repository.GetByUsername(Email);
            if (user.IsExisting())
                throw new UserDuplicate(user);
            repository.Save(this);
        }
    }
}
