using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionTimingTests : Tests
    {
        private ServiceTimingData ServiceTiming { get; set; }


        [SetUp]
        public void Setup()
        {
            ServiceTiming = new ServiceTimingData {Duration = 105};
        }


        [Test]
        public void GivenInvalidStartDate_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingCommand("2014-02-31", "12:45", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The startDate field is not valid.", "session.timing.startDate");
        }

        [Test]
        public void GivenInvalidStartTime_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingCommand("2014-02-28", "25:00", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The startTime field is not valid.", "session.timing.startTime");
        }

        [Test]
        public void GivenInvalidDuration_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingCommand("2014-02-28", "23:00", 69);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The duration field is not valid.", "session.timing.duration");
        }

        [Test]
        public void GivenDurationMissingInServiceAndSession_WhenConstruct_ThenThrowValidationException()
        {
            ServiceTiming.Duration = null;
            var sessionTiming = new SessionTimingCommand("2014-12-31", "12:45");
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The duration field is required.", "session.timing.duration");
        }

        [Test]
        public void GivenDurationMissingInService_WhenConstruct_ThenUseSessionDuration()
        {
            ServiceTiming.Duration = null;
            var sessionTiming = new SessionTimingCommand("2014-12-31", "12:45", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSessionTiming(response, "2014-12-31", "12:45", 75);
        }

        [Test]
        public void GivenDurationMissingInSession_WhenConstruct_ThenUseServiceDuration()
        {
            var sessionTiming = new SessionTimingCommand("2014-12-31", "12:45");
            var response = WhenConstruct(sessionTiming);
            AssertSessionTiming(response, "2014-12-31", "12:45", 105);
        }

        [Test]
        public void GivenDurationInServiceAndSession_WhenConstruct_ThenUseSessionDuration()
        {
            var sessionTiming = new SessionTimingCommand("2014-12-31", "12:45", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSessionTiming(response, "2014-12-31", "12:45", 75);
        }

        [Test]
        public void GivenMultipleErrorsInTiming_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            var sessionTiming = new SessionTimingCommand("2014-15-15", "27:45", 35);
            var response = WhenConstruct(sessionTiming);
            AssertMultipleErrors(response, new[,] { { "The startDate field is not valid.", "session.timing.startDate" },
                                                    { "The startTime field is not valid.", "session.timing.startTime" },
                                                    { "The duration field is not valid.", "session.timing.duration" } });
        }


        private object WhenConstruct(SessionTimingCommand data)
        {
            try
            {
                return new SessionTiming(data, ServiceTiming);
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
