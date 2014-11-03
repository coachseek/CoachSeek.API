using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ServiceRepetitionTests
    {
        [Test]
        public void GivenInvalidRepeatFrequency_WhenConstruct_ThenErrorWithInvalidRepeatFrequency()
        {
            var data = GivenInvalidRepeatFrequency();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithSingleError(response, "service.repetition.repeatFrequency", "The repeatFrequency is not valid.");
        }
        [Test]
        public void GivenInvalidRepeatTimes_WhenConstruct_ThenErrorWithInvalidRepeatTimes()
        {
            var data = GivenInvalidRepeatTimes();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithSingleError(response, "service.repetition.repeatTimes", "The repeatTimes is not valid.");
        }

        [Test]
        public void GivenValidServiceRepetition_WhenConstruct_ThenCreateServiceRepetition()
        {
            var data = GivenValidServiceRepetition();
            var response = WhenConstruct(data);
            ThenCreateServiceRepetition(response);
        }

        [Test]
        public void GivenMultipleInvalidServiceRepetitionErrors_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var data = GivenMultipleInvalidServiceRepetitionErrors();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithMultipleErrors(response);
        }


        private ServiceRepetitionData GivenInvalidRepeatFrequency()
        {
            return new ServiceRepetitionData
            {
                RepeatFrequency = null,
                RepeatTimes = 8
            };
        }

        private ServiceRepetitionData GivenInvalidRepeatTimes()
        {
            return new ServiceRepetitionData
            {
                RepeatFrequency = "d",
                RepeatTimes = -1
            };
        }

        private ServiceRepetitionData GivenValidServiceRepetition()
        {
            return new ServiceRepetitionData
            {
                RepeatFrequency = "W ",
                RepeatTimes = 5
            };
        }

        private ServiceRepetitionData GivenMultipleInvalidServiceRepetitionErrors()
        {
            return new ServiceRepetitionData
            {
                RepeatFrequency = "x",
                RepeatTimes = -6
            };
        }


        private object WhenConstruct(ServiceRepetitionData data)
        {
            try
            {
                return new ServiceRepetition(data);
            }
            catch (Exception ex)
            {
                return ex;
            }
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

        private void ThenCreateServiceRepetition(object response)
        {
            Assert.That(response, Is.InstanceOf<ServiceRepetition>());
            var repetition = (ServiceRepetition)response;
            Assert.That(repetition, Is.Not.Null);
            Assert.That(repetition.RepeatFrequency, Is.EqualTo("w"));
            Assert.That(repetition.RepeatTimes, Is.EqualTo(5));
        }

        private void ThenThrowValidationExceptionWithMultipleErrors(object response)
        {
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = (ValidationException)response;
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Errors.Count, Is.EqualTo(2));
            var firstError = errors.Errors[0];
            Assert.That(firstError.Field, Is.EqualTo("service.repetition.repeatFrequency"));
            Assert.That(firstError.Message, Is.EqualTo("The repeatFrequency is not valid."));
            var secondError = errors.Errors[1];
            Assert.That(secondError.Field, Is.EqualTo("service.repetition.repeatTimes"));
            Assert.That(secondError.Message, Is.EqualTo("The repeatTimes is not valid."));
        }
    }
}
