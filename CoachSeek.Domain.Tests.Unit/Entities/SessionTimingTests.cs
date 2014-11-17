using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionTimingTests
    {
        private const string SERVICE_ID = "F1524EFA-C5A9-4731-8E56-E0BD6DC388D2";
                                          
        private ServiceData Service { get; set; }


        [SetUp]
        public void Setup()
        {
            Service = new ServiceData
            {
                Id = new Guid(SERVICE_ID),
                Name = "Mini Red",
                Description = "Mini Red Service",
                Repetition = new RepetitionData { RepeatTimes = 1 },
                Defaults = new ServiceDefaultsData { Duration = 105, Colour = "Red" },
                Booking = new ServiceBookingData { StudentCapacity = 17, IsOnlineBookable = true },
                Pricing = new PricingData { SessionPrice = 25 }
            };
        }


        [Test]
        public void GivenInvalidStartDate_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingData("2014-02-31", "12:45", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The startDate is not valid.", "session.timing.startDate");
        }

        [Test]
        public void GivenInvalidStartTime_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingData("2014-02-28", "25:00", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The startTime is not valid.", "session.timing.startTime");
        }

        [Test]
        public void GivenInvalidDuration_WhenConstruct_ThenThrowValidationException()
        {
            var sessionTiming = new SessionTimingData("2014-02-28", "23:00", 69);
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The duration is not valid.", "session.timing.duration");
        }

        [Test]
        public void GivenDurationMissingInServiceAndSession_WhenConstruct_ThenThrowValidationException()
        {
            Service.Defaults.Duration = null;
            var sessionTiming = new SessionTimingData("2014-12-31", "12:45");
            var response = WhenConstruct(sessionTiming);
            AssertSingleError(response, "The duration is required.", "session.timing.duration");
        }

        [Test]
        public void GivenDurationMissingInService_WhenConstruct_ThenUseSessionDuration()
        {
            Service.Defaults.Duration = null;
            var sessionTiming = new SessionTimingData("2014-12-31", "12:45", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSessionTiming(response, "2014-12-31", "12:45", 75);
        }

        [Test]
        public void GivenDurationMissingInSession_WhenConstruct_ThenUseServiceDuration()
        {
            var sessionTiming = new SessionTimingData("2014-12-31", "12:45");
            var response = WhenConstruct(sessionTiming);
            AssertSessionTiming(response, "2014-12-31", "12:45", 105);
        }

        [Test]
        public void GivenDurationInServiceAndSession_WhenConstruct_ThenUseSessionDuration()
        {
            var sessionTiming = new SessionTimingData("2014-12-31", "12:45", 75);
            var response = WhenConstruct(sessionTiming);
            AssertSessionTiming(response, "2014-12-31", "12:45", 75);
        }


        private object WhenConstruct(SessionTimingData data)
        {
            try
            {
                return new SessionTiming(data, Service);
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
