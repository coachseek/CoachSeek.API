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
        public void GivenOpenEndedService_WhenConstruct_ThenCreateOpenEndedServiceRepetition()
        {
            var data = GivenOpenEndedService("2W");
            var response = WhenConstruct(data);
            ThenCreateOpenEndedServiceRepetition(response, "2w");
        }

        [Test]
        public void GivenValidServiceRepetition_WhenConstruct_ThenCreateFiniteServiceRepetition()
        {
            var data = GivenValidServiceRepetition("W ", 5);
            var response = WhenConstruct(data);
            ThenCreateFiniteServiceRepetition(response, "w", 5);
        }

        [Test]
        public void GivenMultipleInvalidServiceRepetitionErrors_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var data = GivenMultipleInvalidServiceRepetitionErrors();
            var response = WhenConstruct(data);
            ThenThrowValidationExceptionWithMultipleErrors(response);
        }


        private RepetitionData GivenInvalidRepeatFrequency()
        {
            return new RepetitionData
            {
                RepeatFrequency = null,
                RepeatTimes = 8
            };
        }

        private RepetitionData GivenInvalidRepeatTimes()
        {
            return new RepetitionData
            {
                RepeatFrequency = "d",
                RepeatTimes = -2
            };
        }

        private RepetitionData GivenOpenEndedService(string repeatFrequency)
        {
            return new RepetitionData
            {
                RepeatFrequency = repeatFrequency,
                RepeatTimes = -1        // -1 is for open-ended.
            };
        }

        private RepetitionData GivenValidServiceRepetition(string repeatFrequency, int repeatTimes)
        {
            return new RepetitionData
            {
                RepeatFrequency = repeatFrequency,
                RepeatTimes = repeatTimes
            };
        }

        private RepetitionData GivenMultipleInvalidServiceRepetitionErrors()
        {
            return new RepetitionData
            {
                RepeatFrequency = "x",
                RepeatTimes = -6
            };
        }


        private object WhenConstruct(RepetitionData data)
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

        private void ThenCreateOpenEndedServiceRepetition(object response, string repeatFrequency)
        {
            var repetition = AssertServiceRepetition(response);
            Assert.That(repetition.RepeatFrequency, Is.EqualTo(repeatFrequency));
            Assert.That(repetition.RepeatTimes, Is.EqualTo(-1));
            Assert.That(repetition.IsOpenEnded, Is.True);
        }

        private void ThenCreateFiniteServiceRepetition(object response, string repeatFrequency, int? repeatTimes)
        {
            var repetition = AssertServiceRepetition(response);
            Assert.That(repetition.RepeatFrequency, Is.EqualTo(repeatFrequency));
            Assert.That(repetition.RepeatTimes, Is.EqualTo(repeatTimes));
            Assert.That(repetition.IsOpenEnded, Is.False);
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

        private ServiceRepetition AssertServiceRepetition(object response)
        {
            Assert.That(response, Is.InstanceOf<ServiceRepetition>());
            var repetition = (ServiceRepetition)response;
            Assert.That(repetition, Is.Not.Null);

            return repetition;
        }
    }
}
