using System;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void GivenNoFirstName_WhenConstructNewCustomer_ThenThrowFirstNameRequired()
        {
            var command = GivenNoFirstName();
            var error = WhenConstructNewCustomer(command);
            ThenThrowFirstNameRequired(error);
        }

        [Test]
        public void GivenNoLastName_WhenConstructNewCustomer_ThenThrowLastNameRequired()
        {
            var command = GivenNoLastName();
            var error = WhenConstructNewCustomer(command);
            ThenThrowLastNameRequired(error);
        }

        [Test]
        public void GivenInvalidEmailAddress_WhenConstructNewCustomer_ThenThrowInvalidEmailAddressFormat()
        {
            var command = GivenInvalidEmailAddress();
            var error = WhenConstructNewCustomer(command);
            ThenThrowInvalidEmailAddressFormat(error);
        }

        [Test]
        public void GivenInvalidDateOfBirth_WhenConstructNewCustomer_ThenThrowInvalidDateOfBirth()
        {
            var command = GivenInvalidDateOfBirth();
            var error = WhenConstructNewCustomer(command);
            ThenThrowInvalidDateOfBirth(error);
        }

        [Test]
        public void GivenMultipleErrors_WhenConstructNewCustomer_ThenThrowMultipleErrors()
        {
            var command = GivenMultipleErrors();
            var error = WhenConstructNewCustomer(command);
            ThenThrowMultipleErrors(error);
        }


        private CustomerAddCommand GivenValidCustomerAddCommand()
        {
            return new CustomerAddCommand
            {
                FirstName = "Olaf",
                LastName = "Thielke",
                Email = "olaft@ihug.co.nz",
                Phone = "021430003",
                DateOfBirth = "1970-02-01"
            };
        }

        private CustomerAddCommand GivenNoFirstName()
        {
            var command = GivenValidCustomerAddCommand();
            command.FirstName = null;
            return command;
        }

        private CustomerAddCommand GivenNoLastName()
        {
            var command = GivenValidCustomerAddCommand();
            command.LastName = null;
            return command;
        }

        private CustomerAddCommand GivenInvalidEmailAddress()
        {
            var command = GivenValidCustomerAddCommand();
            command.Email = "abc123";
            return command;
        }

        private CustomerAddCommand GivenInvalidDateOfBirth()
        {
            var command = GivenValidCustomerAddCommand();
            command.DateOfBirth = "1870-02-01";
            return command;
        }

        private CustomerAddCommand GivenMultipleErrors()
        {
            return new CustomerAddCommand
            {
                FirstName = "",
                LastName = "",
                Email = "olaftihugconz",
                Phone = "021430003",
                DateOfBirth = "1870-02-01"
            };
        }


        private object WhenConstructNewCustomer(CustomerAddCommand command)
        {
            try
            {
                return new Customer(command);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenThrowFirstNameRequired(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException) response;
            var errors = exception.Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            AssertFirstNameRequiredError(errors[0]);
        }

        private void ThenThrowLastNameRequired(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException)response;
            var errors = exception.Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            AssertLastNameRequiredError(errors[0]);
        }

        private void ThenThrowInvalidEmailAddressFormat(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException)response;
            var errors = exception.Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            AssertInvalidEmailAddressFormatError(errors[0]);
        }

        private void ThenThrowInvalidDateOfBirth(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException)response;
            var errors = exception.Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            AssertInvalidDateOfBirthError(errors[0]);
        }

        private void ThenThrowMultipleErrors(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException)response;
            var errors = exception.Errors;
            Assert.That(errors.Count, Is.EqualTo(4));
            AssertFirstNameRequiredError(errors[0]);
            AssertLastNameRequiredError(errors[1]);
            AssertInvalidEmailAddressFormatError(errors[2]);
            AssertInvalidDateOfBirthError(errors[3]);
        }


        private void AssertFirstNameRequiredError(Error error)
        {
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.FirstNameRequired));
            Assert.That(error.Message, Is.EqualTo("The FirstName field is required."));
            Assert.That(error.Data, Is.Null);            
        }

        private void AssertLastNameRequiredError(Error error)
        {
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.LastNameRequired));
            Assert.That(error.Message, Is.EqualTo("The LastName field is required."));
            Assert.That(error.Data, Is.Null);
        }

        private void AssertInvalidEmailAddressFormatError(Error error)
        {
            Assert.That(error.Code, Is.EqualTo("email-invalid"));
            Assert.That(error.Message, Is.EqualTo("The Email field is not a valid e-mail address."));
            Assert.That(error.Data, Is.Null);
        }

        private void AssertInvalidDateOfBirthError(Error error)
        {
            Assert.That(error.Code, Is.EqualTo(ErrorCodes.DateOfBirthInvalid));
            Assert.That(error.Message, Is.EqualTo("'1870-02-01' is not a valid date of birth."));
            Assert.That(error.Data, Is.EqualTo("1870-02-01"));
        }
    }
}
