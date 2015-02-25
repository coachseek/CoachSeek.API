﻿using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Name { get { return Person.Name; } }
        public string Email { get { return EmailAddress.Email; } }
        public string Phone { get { return PhoneNumber.Phone; } }

        private PersonName Person { get; set; }
        private EmailAddress EmailAddress { get; set; }
        private PhoneNumber PhoneNumber { get; set; }


        public Customer(Guid id, string firstName, string lastName, string email, string phone)
        {
            Id = id;
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            PhoneNumber = new PhoneNumber(phone);
        }

        public Customer(CustomerData data)
            : this(data.Id,
                   data.FirstName,
                   data.LastName,
                   data.Email,
                   data.Phone)
        { }

        public Customer(CustomerUpdateCommand command)
            : this(command.Id,
                   command.FirstName,
                   command.LastName,
                   command.Email,
                   command.Phone)
        { }


        public CustomerData ToData()
        {
            return Mapper.Map<Customer, CustomerData>(this);
        }

        public CustomerKeyData ToKeyData()
        {
            return Mapper.Map<Customer, CustomerKeyData>(this);
        }
    }
}
