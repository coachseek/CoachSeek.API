using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionTimingTests : Tests
    {
        [Test]
        public void GivenInvalidStartDate_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingCommand("2014-02-31", "12:45", 75, 100);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response,
                              ErrorCodes.StartDateInvalid,
                              "'2014-02-31' is not a valid start date.",
                              "2014-02-31");
        }

        [Test]
        public void GivenInvalidStartTime_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingCommand("2014-02-28", "25:00", 75, 100);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, 
                              ErrorCodes.StartTimeInvalid,
                              "'25:00' is not a valid start time.", 
                              "25:00");
        }

        [Test]
        public void GivenInvalidDuration_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingCommand("2014-02-28", "23:00", 69, 100);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, 
                              ErrorCodes.DurationInvalid,
                              "Duration '69' is not valid.", 
                              "69");
        }

        [Test]
        public void GivenMultipleErrorsInTiming_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var sessionTiming = new SessionTimingCommand("2014-15-15", "27:45", 35, 100);
            var response = WhenConstruct(sessionTiming);
            AssertMultipleErrors(response, new[,] { { ErrorCodes.StartDateInvalid, "'2014-15-15' is not a valid start date.", "2014-15-15", null },
                                                    { ErrorCodes.StartTimeInvalid, "'27:45' is not a valid start time.", "27:45", null },
                                                    { ErrorCodes.DurationInvalid, "Duration '35' is not valid.", "35", null } });
        }


        private object WhenConstruct(SessionTimingCommand data)
        {
            try
            {
                return new SessionTiming(data);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void AssertSessionTiming(object response, string startDate, string startTime, int duration)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionTiming>());
            var timing = ((SessionTiming)response);
            Assert.That(timing.StartDate, Is.EqualTo(startDate));
            Assert.That(timing.StartTime, Is.EqualTo(startTime));
            Assert.That(timing.Duration, Is.EqualTo(duration));
        }
    }
}
