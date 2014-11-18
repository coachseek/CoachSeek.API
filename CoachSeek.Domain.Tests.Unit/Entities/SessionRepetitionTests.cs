﻿using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionRepetitionTests : Tests
    {
        [Test]
        public void GivenInvalidRepeatTimes_WhenConstruct_ThenThrowValidationException()
        {
            var serviceRepetition = new RepetitionData(1);
            var sessionRepetition = new RepetitionData(-10, "w");
            var response = WhenConstruct(sessionRepetition, serviceRepetition);
            AssertSingleError(response, "The repeatTimes field is not valid.", "session.repetition.repeatTimes");
        }

        [Test]
        public void GivenInvalidRepeatFrequency_WhenConstruct_ThenThrowValidationException()
        {
            var serviceRepetition = new RepetitionData(5, "d");
            var sessionRepetition = new RepetitionData(10, "xxx");
            var response = WhenConstruct(sessionRepetition, serviceRepetition);
            AssertSingleError(response, "The repeatFrequency field is not valid.", "session.repetition.repeatFrequency");
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

        [Test]
        public void GivenMultipleErrorsInSessionRepetition_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var sessionRepetition = new RepetitionData(-3, "abc");
            var response = WhenConstruct(sessionRepetition, null);
            AssertMultipleErrors(response, new[,] { { "The repeatTimes field is not valid.", "session.repetition.repeatTimes" },
                                                    { "The repeatFrequency field is not valid.", "session.repetition.repeatFrequency" } });
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
