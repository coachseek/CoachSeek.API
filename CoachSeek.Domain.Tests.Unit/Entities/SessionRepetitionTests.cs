using CoachSeek.Common;
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
            AssertSingleError(response, 
                              ErrorCodes.SessionCountInvalid,              
                              "The SessionCount field is not valid.",
                              "-10");
        }

        [Test]
        public void GivenInvalidRepeatFrequency_WhenConstruct_ThenThrowValidationException()
        {
            var sessionRepetition = new RepetitionCommand(10, "xxx");
            var response = WhenConstruct(sessionRepetition);
            AssertSingleError(response, 
                              ErrorCodes.RepeatFrequencyInvalid, 
                              "The RepeatFrequency field is not valid.", 
                              "xxx");
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
            AssertMultipleErrors(response, new[,] { { ErrorCodes.SessionCountInvalid, "The SessionCount field is not valid.", "-3", null },
                                                    { ErrorCodes.RepeatFrequencyInvalid, "The RepeatFrequency field is not valid.", "abc", null } });
        }

        [Test]
        public void GivenDailyRepeatSessionCountTooLarge_WhenConstruct_ThenThrowValidationExceptions()
        {
            var sessionRepetition = new RepetitionCommand(100, "d");
            var response = WhenConstruct(sessionRepetition);
            AssertSingleError(response,
                              ErrorCodes.CourseExceedsMaximumNumberOfDailySessions,
                              "100 exceeds the maximum number of daily sessions in a course of 30.",
                              "Maximum Allowed Daily Session Count: 30; Specified Session Count: 100");
        }

        [Test]
        public void GivenWeeklyRepeatSessionCountTooLarge_WhenConstruct_ThenThrowValidationExceptions()
        {
            var sessionRepetition = new RepetitionCommand(90, "w");
            var response = WhenConstruct(sessionRepetition);
            AssertSingleError(response, 
                              ErrorCodes.CourseExceedsMaximumNumberOfWeeklySessions,
                              "90 exceeds the maximum number of weekly sessions in a course of 26.",
                              "Maximum Allowed Weekly Session Count: 26; Specified Session Count: 90");
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
