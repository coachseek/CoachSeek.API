using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionBookingTests : Tests
    {
        private ServiceBookingData ServiceBooking { get; set; }


        [SetUp]
        public void Setup()
        {
            ServiceBooking = new ServiceBookingData {StudentCapacity = 17, IsOnlineBookable = true};
        }


        [Test]
        public void GivenNoSessionBooking_WhenConstruct_ThenUseServiceBooking()
        {
            var response = WhenConstruct(null);
            AssertSessionBooking(response, 17, true);
        }

        [Test]
        public void GivenInvalidStudentCapacity_WhenConstruct_ThenThrowValidationException()
        {
            var sessionBooking = new SessionBookingData(-3);
            var response = WhenConstruct(sessionBooking);
            AssertSingleError(response, "The studentCapacity field is not valid.", "session.booking.studentCapacity");
        }

        [Test]
        public void GivenStudentCapacityMissingInServiceAndSession_WhenConstruct_ThenThrowValidationException()
        {
            ServiceBooking.StudentCapacity = null;
            var sessionBooking = new SessionBookingData(null, true);
            var response = WhenConstruct(sessionBooking);
            AssertSingleError(response, "The studentCapacity field is required.", "session.booking.studentCapacity");
        }

        [Test]
        public void GivenStudentCapacityMissingInService_WhenConstruct_ThenUseSessionStudentCapacity()
        {
            ServiceBooking.StudentCapacity = null;
            var sessionBooking = new SessionBookingData(12, true);
            var response = WhenConstruct(sessionBooking);
            AssertSessionBooking(response, 12, true);
        }

        [Test]
        public void GivenStudentCapacityMissingInSession_WhenConstruct_ThenUseServiceStudentCapacity()
        {
            var sessionBooking = new SessionBookingData(null, true);
            var response = WhenConstruct(sessionBooking);
            AssertSessionBooking(response, 17, true);
        }

        [Test]
        public void GivenStudentCapacityInServiceAndSession_WhenConstruct_ThenUseSessionStudentCapacity()
        {
            var sessionBooking = new SessionBookingData(9, false);
            var response = WhenConstruct(sessionBooking);
            AssertSessionBooking(response, 9, false);
        }

        [Test]
        public void GivenIsOnlineBookableMissingInServiceAndSession_WhenConstruct_ThenThrowValidationException()
        {
            ServiceBooking.IsOnlineBookable = null;
            var sessionBooking = new SessionBookingData(13, null);
            var response = WhenConstruct(sessionBooking);
            AssertSingleError(response, "The isOnlineBookable field is required.", "session.booking.isOnlineBookable");
        }

        [Test]
        public void GivenIsOnlineBookableMissingInService_WhenConstruct_ThenUseSessionIsOnlineBookable()
        {
            ServiceBooking.IsOnlineBookable = null;
            var sessionBooking = new SessionBookingData(12, true);
            var response = WhenConstruct(sessionBooking);
            AssertSessionBooking(response, 12, true);
        }

        [Test]
        public void GivenIsOnlineBookableMissingInSession_WhenConstruct_ThenUseServiceIsOnlineBookable()
        {
            var sessionBooking = new SessionBookingData(14, null);
            var response = WhenConstruct(sessionBooking);
            AssertSessionBooking(response, 14, true);
        }

        [Test]
        public void GivenIsOnlineBookableInServiceAndSession_WhenConstruct_ThenUseSessionIsOnlineBookable()
        {
            var sessionBooking = new SessionBookingData(9, false);
            var response = WhenConstruct(sessionBooking);
            AssertSessionBooking(response, 9, false);
        }


        [Test]
        public void GivenMultipleErrorsInSessionBooking_WhenConstruct_ThenThrowValidationExceptionWithMultipleErrors()
        {
            ServiceBooking = null;
            var sessionBooking = new SessionBookingData(-5, null);
            var response = WhenConstruct(sessionBooking);
            AssertMultipleErrors(response, new[,] { { "The studentCapacity field is not valid.", "session.booking.studentCapacity" },
                                                    { "The isOnlineBookable field is required.", "session.booking.isOnlineBookable" } });
        }


        private object WhenConstruct(SessionBookingData data)
        {
            try
            {
                return new SessionBooking(data, ServiceBooking);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void AssertSessionBooking(object response, int studentCapacity, bool isOnlineBookable)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.InstanceOf<SessionBooking>());
            var booking = ((SessionBooking)response);
            Assert.That(booking.StudentCapacity, Is.EqualTo(studentCapacity));
            Assert.That(booking.IsOnlineBookable, Is.EqualTo(isOnlineBookable));
        }
    }
}
