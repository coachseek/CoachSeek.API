using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionRepetitionTests : Tests
    {
        [Test]
        public void GivenInvalidSessionCount_WhenConstruct_ThenThrowValidationException()
        {
            var sessionRepetition = new RepetitionCommand(-10, "w");
            var response = WhenConstruct(sessionRepetition);
            AssertSingleError(response, "The sessionCount field is not valid.", "session.repetition.sessionCount");
        }

        [Test]
        public void GivenInvalidRepeatFrequency_WhenConstruct_ThenThrowValidationException()
        {
            var sessionRepetition = new RepetitionCommand(10, "xxx");
            var response = WhenConstruct(sessionRepetition);
            AssertSingleError(response, "The repeatFrequency field is not valid.", "session.repetition.repeatFrequency");
        }

        [Test]
        public void GivenSessionRepetitionOnly_WhenConstruct_ThenUseSessionRepetition()
        {
            var sessionRepetition = new RepetitionCommand(12, "d");
            var response = WhenConstruct(sessionRepetition);
            AssertSessionRepetition(response, 12, "d");
        }

        [Test]
        public void GivenMultipleErrorsInSessionRepetition_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var sessionRepetition = new RepetitionCommand(-3, "abc");
            var response = WhenConstruct(sessionRepetition);
            AssertMultipleErrors(response, new[,] { { "The sessionCount field is not valid.", "session.repetition.sessionCount" },
                                                    { "The repeatFrequency field is not valid.", "session.repetition.repeatFrequency" } });
        }

        [Test]
        public void GivenDailyRepeatSessionCountTooLarge_WhenConstruct_ThenThrowValidationExceptions()
        {
            var sessionRepetition = new RepetitionCommand(100, "d");
            var response = WhenConstruct(sessionRepetition);
            AssertSingleError(response, "The maximum number of daily sessions is 30.", "session.repetition.sessionCount");
        }

        [Test]
        public void GivenWeeklyRepeatSessionCountTooLarge_WhenConstruct_ThenThrowValidationExceptions()
        {
            var sessionRepetition = new RepetitionCommand(90, "w");
            var response = WhenConstruct(sessionRepetition);
            AssertSingleError(response, "The maximum number of weekly sessions is 26.", "session.repetition.sessionCount");
        }


        private object WhenConstruct(RepetitionCommand sessionRepetition)
        {
            try
            {
                return new SessionRepetition(sessionRepetition);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void AssertSessionRepetition(object response, int sessionCount, string repeatFrequency)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionRepetition>());
            var repetition = ((SessionRepetition)response);
            Assert.That(repetition.SessionCount, Is.EqualTo(sessionCount));
            Assert.That(repetition.RepeatFrequency, Is.EqualTo(repeatFrequency));
        }
    }
}
