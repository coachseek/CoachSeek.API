using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionBookingTests : Tests
    {
        [Test]
        public void GivenInvalidStudentCapacity_WhenConstruct_ThenThrowValidationException()
        {
            var sessionBooking = new SessionBookingCommand(-3, true);
            var response = WhenConstruct(sessionBooking);
            AssertSingleError(response, "The studentCapacity field is not valid.", "session.booking.studentCapacity");
        }

        [Test]
        public void GivenValidStudentCapacity_WhenConstruct_ThenCreateSessionBooking()
        {
            var sessionBooking = new SessionBookingCommand(12, true);
            var response = WhenConstruct(sessionBooking);
            AssertSessionBooking(response, 12, true);
        }


        private object WhenConstruct(SessionBookingCommand data)
        {
            try
            {
                return new SessionBooking(data);
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
