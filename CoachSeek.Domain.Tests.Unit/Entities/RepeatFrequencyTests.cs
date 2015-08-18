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
            RepeatFrequencyCreationFailure("");
            RepeatFrequencyCreationFailure("hello world!");
            RepeatFrequencyCreationFailure("2d");
            RepeatFrequencyCreationFailure(" 2D");
            RepeatFrequencyCreationFailure("2w");
            RepeatFrequencyCreationFailure(" 2W");
            RepeatFrequencyCreationFailure("3d");
            RepeatFrequencyCreationFailure("3w");
            RepeatFrequencyCreationFailure("m");
            RepeatFrequencyCreationFailure("M ");
            RepeatFrequencyCreationFailure("2m");

            RepeatFrequencyCreationSuccess(null);   // RepeatFrequency must be null for single session.
            RepeatFrequencyCreationSuccess("d");
            RepeatFrequencyCreationSuccess("D ");
            RepeatFrequencyCreationSuccess("w ");
            RepeatFrequencyCreationSuccess("  W");
        }


        private void RepeatFrequencyCreationSuccess(string inputFrequnecy)
        {
            var frequency = new RepeatFrequency(inputFrequnecy);
            Assert.That(frequency, Is.Not.Null);
            if (inputFrequnecy == null)
                Assert.That(frequency.Frequency, Is.Null);
            else
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
                Assert.That(ex, Is.TypeOf<RepeatFrequencyInvalid>());
            }
        }
    }
}
