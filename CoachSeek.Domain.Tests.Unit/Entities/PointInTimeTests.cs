using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class PointInTimeTests
    {
        [Test]
        public void GivenNullTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenNullTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenEmptyTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenEmptyTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenWhitespaceTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenWhitespaceTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenRandomTextTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenRandomTextTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenRandomTextWithColonTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenRandomTextWithColonTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenTooBigHourTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooBigHourTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenTooBigMinuteTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooBigMinuteTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenTooSmallHourTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooSmallHourTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenTooSmallMinuteTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooSmallMinuteTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenNonQuarterOfHourTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenNonQuarterOfHourTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidPointInTime(respond);
        }

        [Test]
        public void GivenValidTimeString_WhenConstruct_ThenCreatePointInTime()
        {
            var timeString = GivenValidTimeString();
            var respond = WhenConstruct(timeString);
            ThenCreatePointInTime(respond);
        }

        [Test]
        public void GivenMidnightTimeString_WhenConstruct_ThenCreateMidnightPointInTime()
        {
            var timeString = GivenMidnightTimeString();
            var respond = WhenConstruct(timeString);
            ThenCreateMidnightPointInTime(respond);
        }

        [Test]
        public void TestIsAfter()
        {
            AssertIsAfter("9:00", "8:45", true);
            AssertIsAfter("9:00", "10:00", false);
            AssertIsAfter("9:00", "9:15", false);
            AssertIsAfter("9:00", "9:00", false);
        }

        [Test]
        public void TestToData()
        {
            AssertToData("0:00");
            AssertToData("0:15");
            AssertToData("0:30");
            AssertToData("0:45");
            AssertToData("1:00");

            AssertToData("6:00");
            AssertToData("6:15");
            AssertToData("6:30");
            AssertToData("6:45");

            AssertToData("10:00");
            AssertToData("10:15");
            AssertToData("10:30");
            AssertToData("10:45");

            AssertToData("23:00");
            AssertToData("23:15");
            AssertToData("23:30");
            AssertToData("23:45");
        }


        private string GivenNullTimeString()
        {
            return null;
        }

        private string GivenEmptyTimeString()
        {
            return "";
        }

        private string GivenWhitespaceTimeString()
        {
            return "      ";
        }

        private string GivenRandomTextTimeString()
        {
            return "hello world!";
        }

        private string GivenRandomTextWithColonTimeString()
        {
            return "random:data";
        }

        private string GivenTooBigHourTimeString()
        {
            return "24:00";
        }

        private string GivenTooBigMinuteTimeString()
        {
            return "17:60";
        }

        private string GivenTooSmallHourTimeString()
        {
            return "-4:15";
        }

        private string GivenTooSmallMinuteTimeString()
        {
            return "7:-5";
        }

        private string GivenNonQuarterOfHourTimeString()
        {
            return "8:26";
        }

        private string GivenValidTimeString()
        {
            return "9:45";
        }

        private string GivenMidnightTimeString()
        {
            return "0:00";
        }


        private object WhenConstruct(string timeString)
        {
            try
            {
                return new PointInTime(timeString);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenThrowInvalidPointInTime(object response)
        {
            Assert.That(response, Is.InstanceOf<InvalidPointInTime>());
        }

        private void ThenCreatePointInTime(object response)
        {
            Assert.That(response, Is.InstanceOf<PointInTime>());
            var pointInTime = (PointInTime)response;
            Assert.That(pointInTime.Hour, Is.EqualTo(9));
            Assert.That(pointInTime.Minute, Is.EqualTo(45));
        }

        private void ThenCreateMidnightPointInTime(object response)
        {
            Assert.That(response, Is.InstanceOf<PointInTime>());
            var pointInTime = (PointInTime)response;
            Assert.That(pointInTime.Hour, Is.EqualTo(0));
            Assert.That(pointInTime.Minute, Is.EqualTo(0));
        }

        private void AssertIsAfter(string later, string earlier, bool isAfter)
        {
            var laterPit = new PointInTime(later);
            var earlierPit = new PointInTime(earlier);

            Assert.That(laterPit.IsAfter(earlierPit), Is.EqualTo(isAfter));
        }

        private void AssertToData(string time)
        {
            Assert.That(new PointInTime(time).ToData(), Is.EqualTo(time));
        }


        public class TimePoints
        {
            public PointInTime Earlier { get; set; }
            public PointInTime Later { get; set; }

            public TimePoints(string earlier, string later)
            {
                Earlier = new PointInTime(earlier);
                Later = new PointInTime(later);
            }
        }
    }
}
