using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class RepeatTimesTests
    {
        [Test]
        public void RepeatTimesCreationTests()
        {
            RepeatTimesCreationFailure(-50);
            RepeatTimesCreationFailure(-5);
            RepeatTimesCreationFailure(-3);
            RepeatTimesCreationFailure(-2);
            RepeatTimesCreationFailure(0);

            RepeatTimesCreationSuccess(-1); // -1 is used for Open-Ended.
            RepeatTimesCreationSuccess(1);
            RepeatTimesCreationSuccess(2);
            RepeatTimesCreationSuccess(3);
            RepeatTimesCreationSuccess(5);
            RepeatTimesCreationSuccess(10);
            RepeatTimesCreationSuccess(100);
            RepeatTimesCreationSuccess(1000);
        }


        private void RepeatTimesCreationSuccess(int inputTimes)
        {
            var times = new RepeatTimes(inputTimes);
            Assert.That(times, Is.Not.Null);
            Assert.That(times.Count, Is.EqualTo(inputTimes));
        }

        private void RepeatTimesCreationFailure(int inputTimes)
        {
            try
            {
                var times = new RepeatTimes(inputTimes);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidRepeatTimes>());
            }
        }
    }
}
