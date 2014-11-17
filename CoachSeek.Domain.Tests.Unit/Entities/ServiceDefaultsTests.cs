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
                Colour = "Blue",
            };
        }

        private ServiceDefaultsData GivenInvalidDuration()
        {
            var defaults = GivenValidServiceDefaults();
            defaults.Duration = -9;

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
                Colour = "Ochre",
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
            Assert.That(defaults.Colour, Is.Null);
        }

        private void ThenCreateServiceDefaults(object response)
        {
            Assert.That(response, Is.InstanceOf<ServiceDefaults>());
            var defaults = (ServiceDefaults)response;
            Assert.That(defaults, Is.Not.Null);
            Assert.That(defaults.Duration, Is.EqualTo(75));
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
            Assert.That(errors.Errors.Count, Is.EqualTo(2));
            var firstError = errors.Errors[0];
            Assert.That(firstError.Field, Is.EqualTo("service.defaults.duration"));
            Assert.That(firstError.Message, Is.EqualTo("The duration is not valid."));
            var secondError = errors.Errors[1];
            Assert.That(secondError.Field, Is.EqualTo("service.defaults.colour"));
            Assert.That(secondError.Message, Is.EqualTo("The colour is not valid."));
        }
    }
}
