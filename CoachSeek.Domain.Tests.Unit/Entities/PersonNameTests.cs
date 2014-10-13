using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class PersonNameTests
    {
        [Test]
        public void GivenNullFirstName_WhenConstructPersonName_ThenThrowMissingFirstName()
        {
            var names = GivenNullFirstName();
            var response = WhenConstructPersonName(names);
            ThenThrowMissingFirstName(response);
        }
        
        [Test]
        public void GivenNullLastName_WhenConstructPersonName_ThenThrowMissingLastName()
        {
            var names = GivenNullLastName();
            var response = WhenConstructPersonName(names);
            ThenThrowMissingLastName(response);
        }

        [Test]
        public void GivenValidNames_WhenConstructPersonName_ThenConstructPersonName()
        {
            var names = GivenValidNames();
            var response = WhenConstructPersonName(names);
            ThenConstructPersonName(response);
        }


        private Tuple<string, string> GivenNullFirstName()
        {
            return new Tuple<string, string>(null, "Thielke");
        }

        private Tuple<string, string> GivenNullLastName()
        {
            return new Tuple<string, string>("Olaf", null);
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

        private void ThenThrowMissingFirstName(object response)
        {
            Assert.That(response, Is.InstanceOf<MissingFirstName>());
        }

        private void ThenThrowMissingLastName(object response)
        {
            Assert.That(response, Is.InstanceOf<MissingLastName>());
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
