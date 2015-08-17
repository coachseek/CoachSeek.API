using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class EmailAddressTests
    {
        [Test]
        public void GivenNullEmail_WhenConstructEmailAddress_ThenThrowEmailAddressRequired()
        {
            var email = GivenNullEmail();
            var response = WhenConstructEmailAddress(email);
            ThenThrowEmailAddressRequired(response);
        }

        [Test]
        public void GivenEmptyEmail_WhenConstructEmailAddress_ThenThrowEmailAddressFormatInvalid()
        {
            var email = GivenEmptyEmail();
            var response = WhenConstructEmailAddress(email);
            ThenThrowEmailAddressFormatInvalid(response);
        }

        [Test]
        public void GivenWhitespaceEmail_WhenConstructEmailAddress_ThenThrowEmailAddressFormatInvalid()
        {
            var email = GivenWhitespaceEmail();
            var response = WhenConstructEmailAddress(email);
            ThenThrowEmailAddressFormatInvalid(response);
        }

        [Test]
        public void GivenInvalidEmailFormat_WhenConstructEmailAddress_ThenThrowEmailAddressFormatInvalid()
        {
            var email = GivenInvalidEmailFormat();
            var response = WhenConstructEmailAddress(email);
            ThenThrowEmailAddressFormatInvalid(response);
        }

        [Test]
        public void GivenValidEmailFormat_WhenConstructEmailAddress_ThenConstructEmailAddress()
        {
            var email = GivenValidEmailFormat();
            var response = WhenConstructEmailAddress(email);
            ThenConstructEmailAddress(response);
        }


        private string GivenNullEmail()
        {
            return null;
        }

        private string GivenEmptyEmail()
        {
            return "";
        }

        private string GivenWhitespaceEmail()
        {
            return "     ";
        }

        private string GivenInvalidEmailFormat()
        {
            return "fred@";
        }

        private string GivenValidEmailFormat()
        {
            return "  Fred@TEST.Com   ";
        }

        private object WhenConstructEmailAddress(string email)
        {
            try
            {
                return new EmailAddress(email);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private void ThenThrowEmailAddressRequired(object response)
        {
            Assert.That(response, Is.InstanceOf<EmailAddressRequired>());
        }

        private void ThenThrowEmailAddressFormatInvalid(object response)
        {
            Assert.That(response, Is.InstanceOf<EmailAddressFormatInvalid>());
        }

        private void ThenConstructEmailAddress(object response)
        {
            Assert.That(response, Is.InstanceOf<EmailAddress>());
            var emailAddress = (EmailAddress) response;
            Assert.That(emailAddress.Email, Is.EqualTo("fred@test.com"));
        }
    }
}
