using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class ServiceStudentCapacityTests
    {
        [Test]
        public void ServiceStudentCapacityCreationTests()
        {
            StudentCapacityCreationFailure(-12);    // No negative student capacities.
            StudentCapacityCreationFailure(101);    // 100 is max allowed.

            StudentCapacityCreationSuccess(null);   // Ok for Service not to have a StudentCapacity.
            StudentCapacityCreationSuccess(0);
            StudentCapacityCreationSuccess(43);
            StudentCapacityCreationSuccess(100);    // 100 is max allowed.
        }


        private void StudentCapacityCreationSuccess(int? inputMaximum)
        {
            var capacity = new ServiceStudentCapacity(inputMaximum);
            Assert.That(capacity, Is.Not.Null);
            Assert.That(capacity.Maximum, Is.EqualTo(inputMaximum));
        }

        private void StudentCapacityCreationFailure(int? inputMaximum)
        {
            try
            {
                var capacity = new ServiceStudentCapacity(inputMaximum);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf<InvalidStudentCapacity>());
            }
        }
    }
}
