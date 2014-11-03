using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ServiceDefaultsTests
    {
        [Test]
        public void GivenEmptyServiceDefaultsData_WhenConstruct_ThenCreateDegenerateServiceDefaults()
        {
            var data = GivenEmptyServiceDefaultsData();
            var response = WhenConstruct(data);
            ThenCreateDegenerateServiceDefaults(response);
        }

        [Test]
        public void GivenValidServiceDefaults_WhenConstruct_ThenCreateServiceDefaults()
        {
            var data = GivenValidServiceDefaults();
            var response = WhenConstruct(data);
            ThenCreateServiceDefaults(response);
        }

        [Test]
        public void GivenInvalidDuration_WhenConstruct_ThenThrowValidationExceptionWithSingleError()
        {
            var data = GivenInvalidDuration();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithSingleError(response, "service.defaults.duration", "The duration is not valid.");
        }

        [Test]
        public void GivenInvalidPrice_WhenConstruct_ThenThrowValidationExceptionWithSingleError()
        {
            var data = GivenInvalidPrice();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithSingleError(response, "service.defaults.price", "The price is not valid.");
        }

        [Test]
        public void GivenInvalidStudentCapacity_WhenConstruct_ThenThrowValidationExceptionWithSingleError()
        {
            var data = GivenInvalidStudentCapacity();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithSingleError(response, "service.defaults.studentCapacity", "The studentCapacity is not valid.");
        }

        [Test]
        public void GivenInvalidColour_WhenConstruct_ThenThrowValidationExceptionWithSingleError()
        {
            var data = GivenInvalidColour();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithSingleError(response, "service.defaults.colour", "The colour is not valid.");
        }

        [Test]
        public void GivenMultipleInvalidServiceDefaults_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var data = GivenMultipleInvalidServiceDefaults();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithMultipleErrors(response);
        }


        private ServiceDefaultsData GivenEmptyServiceDefaultsData()
        {
            return new ServiceDefaultsData();
        }

        private ServiceDefaultsData GivenValidServiceDefaults()
        {
            return new ServiceDefaultsData
            {
                Duration = 75,
                Price = 123.45m,
                StudentCapacity = 13,
                Colour = "Blue",
                IsOnlineBookable = true
            };
        }

        private ServiceDefaultsData GivenInvalidDuration()
        {
            var defaults = GivenValidServiceDefaults();
            defaults.Duration = -9;

            return defaults;
        }

        private ServiceDefaultsData GivenInvalidPrice()
        {
            var defaults = GivenValidServiceDefaults();
            defaults.Price = 12.3456m;

            return defaults;
        }

        private ServiceDefaultsData GivenInvalidStudentCapacity()
        {
            var defaults = GivenValidServiceDefaults();
            defaults.StudentCapacity = -5;

            return defaults;
        }

        private ServiceDefaultsData GivenInvalidColour()
        {
            var defaults = GivenValidServiceDefaults();
            defaults.Colour = "Magenta";

            return defaults;
        }

        private ServiceDefaultsData GivenMultipleInvalidServiceDefaults()
        {
            return new ServiceDefaultsData
            {
                Duration = 7665,
                Price = 66.6667m,
                StudentCapacity = -2,
                Colour = "Ochre",
                IsOnlineBookable = null
            };
        }


        private object WhenConstruct(ServiceDefaultsData data)
        {
            try
            {
                return new ServiceDefaults(data);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenCreateDegenerateServiceDefaults(object response)
        {
            Assert.That(response, Is.InstanceOf<ServiceDefaults>());
            var defaults = (ServiceDefaults) response;
            Assert.That(defaults, Is.Not.Null);
            Assert.That(defaults.Duration, Is.Null);
            Assert.That(defaults.Price, Is.Null);
            Assert.That(defaults.StudentCapacity, Is.Null);
            Assert.That(defaults.IsOnlineBookable, Is.Null);
            Assert.That(defaults.Colour, Is.Null);
        }

        private void ThenCreateServiceDefaults(object response)
        {
            Assert.That(response, Is.InstanceOf<ServiceDefaults>());
            var defaults = (ServiceDefaults)response;
            Assert.That(defaults, Is.Not.Null);
            Assert.That(defaults.Duration, Is.EqualTo(75));
            Assert.That(defaults.Price, Is.EqualTo(123.45m));
            Assert.That(defaults.StudentCapacity, Is.EqualTo(13));
            Assert.That(defaults.IsOnlineBookable, Is.EqualTo(true));
            Assert.That(defaults.Colour, Is.EqualTo("blue"));
        }

        private void ThenThrowValidationExceptionWithSingleError(object response, string field, string message)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = (ValidationException)response;
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Errors.Count, Is.EqualTo(1));
            var error = errors.Errors[0];
            Assert.That(error.Field, Is.EqualTo(field));
            Assert.That(error.Message, Is.EqualTo(message));
        }

        private void ThenThrowValidationExceptionWithMultipleErrors(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = (ValidationException)response;
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Errors.Count, Is.EqualTo(4));
            var firstError = errors.Errors[0];
            Assert.That(firstError.Field, Is.EqualTo("service.defaults.duration"));
            Assert.That(firstError.Message, Is.EqualTo("The duration is not valid."));
            var secondError = errors.Errors[1];
            Assert.That(secondError.Field, Is.EqualTo("service.defaults.price"));
            Assert.That(secondError.Message, Is.EqualTo("The price is not valid."));
            var thirdError = errors.Errors[2];
            Assert.That(thirdError.Field, Is.EqualTo("service.defaults.studentCapacity"));
            Assert.That(thirdError.Message, Is.EqualTo("The studentCapacity is not valid."));
            var fourthError = errors.Errors[3];
            Assert.That(fourthError.Field, Is.EqualTo("service.defaults.colour"));
            Assert.That(fourthError.Message, Is.EqualTo("The colour is not valid."));
        }
    }
}
