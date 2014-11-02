using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class StudentCapacityTests
    {
        [Test]
        public void StudentCapacityCreationTests()
        {
            StudentCapacityCreationFailure(-12); // No negative student capacities.
            StudentCapacityCreationFailure(101); // 100 is max allowed.

            StudentCapacityCreationSuccess(null);
            StudentCapacityCreationSuccess(0);
            StudentCapacityCreationSuccess(43);
            StudentCapacityCreationSuccess(100); // 100 is max allowed.
        }


        private void StudentCapacityCreationSuccess(int? inputMaximum)
        {
            var capacity = new StudentCapacity(inputMaximum);
            Assert.That(capacity, Is.Not.Null);
            Assert.That(capacity.Maximum, Is.EqualTo(inputMaximum));
        }

        private void StudentCapacityCreationFailure(int? inputMaximum)
        {
            try
            {
                var capacity = new StudentCapacity(inputMaximum);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidStudentCapacity>());
            }
        }
    }
}
