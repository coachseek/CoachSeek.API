using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionRepetitionTests
    {
        [Test]
        public void GivenInvalidRepeatTimes_WhenConstruct_ThenThrowValidationException()
        {
            var serviceRepetition = new RepetitionData(1);
            var sessionRepetition = new RepetitionData(-10, "w");
            var response = WhenConstruct(sessionRepetition, serviceRepetition);
            AssertSingleError(response, "The repeatTimes is not valid.", "session.repetition.repeatTimes");
        }

        [Test]
        public void GivenInvalidRepeatFrequency_WhenConstruct_ThenThrowValidationException()
        {
            var serviceRepetition = new RepetitionData(5, "d");
            var sessionRepetition = new RepetitionData(10, "xxx");
            var response = WhenConstruct(sessionRepetition, serviceRepetition);
            AssertSingleError(response, "The repeatFrequency is not valid.", "session.repetition.repeatFrequency");
        }

        [Test]
        public void GivenSessionRepetitionOnly_WhenConstruct_ThenUseSessionRepetition()
        {
            var sessionRepetition = new RepetitionData(12, "d");
            var response = WhenConstruct(sessionRepetition, null);
            AssertSessionRepetition(response, 12, "d");
        }

        [Test]
        public void GivenServiceRepetitionOnly_WhenConstruct_ThenUseServiceRepetition()
        {
            var serviceRepetition = new RepetitionData(8, "w");
            var response = WhenConstruct(null, serviceRepetition);
            AssertSessionRepetition(response, 8, "w");
        }


        private object WhenConstruct(RepetitionData sessionRepetition, RepetitionData serviceRepetition)
        {
            try
            {
                return new SessionRepetition(sessionRepetition, serviceRepetition);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void AssertSingleError(object response, string message, string field)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<ValidationException>());
            var errors = ((ValidationException)response).Errors;
            Assert.That(errors.Count, Is.EqualTo(1));
            var error = errors[0];
            Assert.That(error.Message, Is.EqualTo(message));
            Assert.That(error.Field, Is.EqualTo(field));
        }

        private void AssertSessionRepetition(object response, int repeatTimes, string repeatFrequency)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionRepetition>());
            var repetition = ((SessionRepetition)response);
            Assert.That(repetition.RepeatTimes, Is.EqualTo(repeatTimes));
            Assert.That(repetition.RepeatFrequency, Is.EqualTo(repeatFrequency));
        }
    }
}
