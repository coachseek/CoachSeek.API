using System;
using AutoMapper;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Name { get { return Person.Name; } }
        public string Email { get { return EmailAddress.IsExisting() ?  EmailAddress.Email : null; } }
        public string Phone { get { return PhoneNumber.IsExisting() ? PhoneNumber.Phone : null; } }
        public string SexType { get { return Sex.IsExisting() ? Sex.SexType : null; } }
        public string DateOfBirth { get { return DOB.IsExisting() ? DOB.ToString() : null; } }

        private PersonName Person { get; set; }
        private EmailAddress EmailAddress { get; set; }
        private PhoneNumber PhoneNumber { get; set; }
        private Sex Sex { get; set; }
        private DateOfBirth DOB { get; set; }


        public Customer(CustomerAddCommand command)
            : this(Guid.NewGuid(), 
                   command.FirstName,
                   command.LastName,
                   command.Email,
                   command.Phone,
                   command.Sex,
                   command.DateOfBirth)
        { }

        public Customer(CustomerUpdateCommand command)
            : this(command.Id,
                   command.FirstName,
                   command.LastName,
                   command.Email,
                   command.Phone,
                   command.Sex,
                   command.DateOfBirth)
        { }

        public Customer(CustomerData data)
            : this(data.Id,
                   data.FirstName,
                   data.LastName,
                   data.Email,
                   data.Phone,
                   data.Sex,
                   data.DateOfBirth)
        { }

        public Customer(Guid id, string firstName, string lastName, string email, string phone, string sex, string dateOfBirth = null)
        {
            Validate(firstName, lastName, email, dateOfBirth);
            SetProperties(id, firstName, lastName, email, phone, sex, dateOfBirth);
        }


        public CustomerData ToData()
        {
            return Mapper.Map<Customer, CustomerData>(this);
        }


        private void SetProperties(Guid id, 
                                   string firstName, 
                                   string lastName, 
                                   string email, 
                                   string phone,
                                   string sex,
                                   string dateOfBirth = null)
        {
            Id = id;
            Person = new PersonName(firstName, lastName);
            if (email.IsExisting())
                EmailAddress = new EmailAddress(email);
            if (phone.IsExisting())
                PhoneNumber = new PhoneNumber(phone);
            if (dateOfBirth.IsExisting())
                DOB = new DateOfBirth(dateOfBirth);
            if (sex.IsExisting())
                Sex = new Sex(sex);
        }

        private void Validate(string firstName, 
                              string lastName,
                              string email, 
                              string dateOfBirth = null)
        {
            var errors = new ValidationException();

            ValidateName(firstName, lastName, errors);
            ValidateEmail(email, errors);
            ValidateDateOfBirth(dateOfBirth, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateName(string firstName, string lastName, ValidationException errors)
        {
            try
            {
                var person = new PersonName(firstName, lastName);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateEmail(string email, ValidationException errors)
        {
            if (!email.IsExisting())
                return;

            try
            {
                var emailAddress = new EmailAddress(email);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        private void ValidateDateOfBirth(string dateOfBirth, ValidationException errors)
        {
            if (!dateOfBirth.IsExisting())
                return;

            try
            {
                var dob = new DateOfBirth(dateOfBirth);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }
    }
}
