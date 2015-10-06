using System;
using CoachSeek.Common;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class PersonNameTests
    {
        [Test]
        public void GivenNoFirstName_WhenConstructPersonName_ThenThrowFirstNameRequired()
        {
            var names = GivenNoFirstName();
            var response = WhenConstructPersonName(names);
            ThenThrowFirstNameRequired(response);
        }
        
        [Test]
        public void GivenNoLastName_WhenConstructPersonName_ThenThrowLastNameRequired()
        {
            var names = GivenNoLastName();
            var response = WhenConstructPersonName(names);
            ThenThrowLastNameRequired(response);
        }

        [Test]
        public void GivenNoFirstNameAndNoLastName_WhenConstructPersonName_ThenThrowFirstNameRequiredAndLastNameRequired()
        {
            var names = GivenNoFirstNameAndNoLastName();
            var response = WhenConstructPersonName(names);
            ThenThrowFirstNameRequiredAndLastNameRequired(response);
        }

        [Test]
        public void GivenValidNames_WhenConstructPersonName_ThenConstructPersonName()
        {
            var names = GivenValidNames();
            var response = WhenConstructPersonName(names);
            ThenConstructPersonName(response);
        }


        private Tuple<string, string> GivenNoFirstName()
        {
            return new Tuple<string, string>(null, "Thielke");
        }

        private Tuple<string, string> GivenNoLastName()
        {
            return new Tuple<string, string>("Olaf", null);
        }

        private Tuple<string, string> GivenNoFirstNameAndNoLastName()
        {
            return new Tuple<string, string>(null, null);
        }

        private Tuple<string, string> GivenValidNames()
        {
            return new Tuple<string, string>("   Olaf ", " Thielke  ");
        }

        private object WhenConstructPersonName(Tuple<string, string> names)
        {
            try
            {
                return new PersonName(names.Item1, names.Item2);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void ThenThrowFirstNameRequired(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException)response;
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

        private void ThenThrowFirstNameRequiredAndLastNameRequired(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var exception = (ValidationException)response;
            var errors = exception.Errors;
            Assert.That(errors.Count, Is.EqualTo(2));
            AssertFirstNameRequiredError(errors[0]);
            AssertLastNameRequiredError(errors[1]);
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

        private void ThenConstructPersonName(object response)
        {
            Assert.That(response, Is.InstanceOf<PersonName>());
            var personName = (PersonName)response;
            Assert.That(personName.FirstName, Is.EqualTo("Olaf"));
            Assert.That(personName.LastName, Is.EqualTo("Thielke"));
            Assert.That(personName.Name, Is.EqualTo("Olaf Thielke"));
        }
    }
}
