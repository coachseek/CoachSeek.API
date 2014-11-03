using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class RepeatFrequencyTests
    {
        [Test]
        public void RepeatFrequencyCreationTests()
        {
            RepeatFrequencyCreationFailure(null);
            RepeatFrequencyCreationFailure("");
            RepeatFrequencyCreationFailure("hello world!");
            RepeatFrequencyCreationFailure("3d");
            RepeatFrequencyCreationFailure("3w");
            RepeatFrequencyCreationFailure("2m");

            RepeatFrequencyCreationSuccess("d");
            RepeatFrequencyCreationSuccess("D ");
            RepeatFrequencyCreationSuccess("2d");
            RepeatFrequencyCreationSuccess(" 2D");
            RepeatFrequencyCreationSuccess("w ");
            RepeatFrequencyCreationSuccess("  W");
            RepeatFrequencyCreationSuccess("2w");
            RepeatFrequencyCreationSuccess(" 2W");
            RepeatFrequencyCreationSuccess("m");
            RepeatFrequencyCreationSuccess("M ");
        }


        private void RepeatFrequencyCreationSuccess(string inputFrequnecy)
        {
            var frequency = new RepeatFrequency(inputFrequnecy);
            Assert.That(frequency, Is.Not.Null);
            Assert.That(frequency.Frequency, Is.EqualTo(inputFrequnecy.Trim().ToLower()));
        }

        private void RepeatFrequencyCreationFailure(string inputFrequnecy)
        {
            try
            {
                var frequency = new RepeatFrequency(inputFrequnecy);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidRepeatFrequency>());
            }
        }
    }
}
