using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class SessionDurationTests
    {
        [Test]
        public void SessionDurationCreationTests()
        {
            SessionDurationCreationFailure(-30);
            SessionDurationCreationFailure(0);
            SessionDurationCreationFailure(23);     // Must be multiple of 15 minutes.
            SessionDurationCreationFailure(666);    // Must be multiple of 15 minutes.
            SessionDurationCreationFailure(1455);   // Greater than 24 hours.

            SessionDurationCreationSuccess(15);
            SessionDurationCreationSuccess(30);
            SessionDurationCreationSuccess(60);
            SessionDurationCreationSuccess(75);
            SessionDurationCreationSuccess(90);
            SessionDurationCreationSuccess(300);
            SessionDurationCreationSuccess(1425);
            SessionDurationCreationSuccess(1440);   // 24 hours.
        }


        private void SessionDurationCreationSuccess(int inputMinutes)
        {
            var duration = new SessionDuration(inputMinutes);
            Assert.That(duration, Is.Not.Null);
            Assert.That(duration.Minutes, Is.EqualTo(inputMinutes));
        }

        private void SessionDurationCreationFailure(int inputMinutes)
        {
            try
            {
                var duration = new SessionDuration(inputMinutes);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<DurationInvalid>());
            }
        }
    }
}
