using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class DurationTests
    {
        [Test]
        public void DurationCreationTests()
        {
            DurationCreationFailure(-30);
            DurationCreationFailure(0);
            DurationCreationFailure(23); // Must be multiple of 15 minutes.
            DurationCreationFailure(666); // Must be multiple of 15 minutes.
            DurationCreationFailure(1455); // Greater than 24 hours.

            DurationCreationSuccess(null);
            DurationCreationSuccess(15);
            DurationCreationSuccess(30);
            DurationCreationSuccess(60);
            DurationCreationSuccess(75);
            DurationCreationSuccess(90);
            DurationCreationSuccess(300);
            DurationCreationSuccess(1425);
            DurationCreationSuccess(1440); // 24 hours.
        }


        private void DurationCreationSuccess(int? inputMinutes)
        {
            var duration = new Duration(inputMinutes);
            Assert.That(duration, Is.Not.Null);
            Assert.That(duration.Minutes, Is.EqualTo(inputMinutes));
        }

        private void DurationCreationFailure(int? inputMinutes)
        {
            try
            {
                var duration = new Duration(inputMinutes);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<DurationInvalid>());
            }
        }
    }
}
