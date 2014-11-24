using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class PointInTimeTests
    {
        [Test]
        public void PointInTimeIsAfterTests()
        {
            AssertIsAfterSuccess("2014-11-24 9:00", "2014-11-24 8:00");
            AssertIsAfterSuccess("2014-11-24 1:00", "2014-11-23 23:00");

            AssertIsAfterFailure("2014-11-24 8:00", "2014-11-24 9:00");
            AssertIsAfterFailure("2014-11-24 9:00", "2014-11-24 9:00");
            AssertIsAfterFailure("2014-11-23 23:00", "2014-11-24 1:00");
        }

        private void AssertIsAfterSuccess(string afterDateTime, string beforeDateTime)
        {
            var afterPointInTime = ParsePointInTime(afterDateTime);
            var beforePointInTime = ParsePointInTime(beforeDateTime);

            var isAfter = afterPointInTime.IsAfter(beforePointInTime);
            Assert.That(isAfter, Is.True);
        }

        private void AssertIsAfterFailure(string afterDateTime, string beforeDateTime)
        {
            var afterPointInTime = ParsePointInTime(afterDateTime);
            var beforePointInTime = ParsePointInTime(beforeDateTime);

            var isAfter = afterPointInTime.IsAfter(beforePointInTime);
            Assert.That(isAfter, Is.False);
        }

        private PointInTime ParsePointInTime(string dateTime)
        {
            var dateTimeArray = dateTime.Split(' ');
            return new PointInTime(new Date(dateTimeArray[0]), new TimeOfDay(dateTimeArray[1]));
        }
    }
}
