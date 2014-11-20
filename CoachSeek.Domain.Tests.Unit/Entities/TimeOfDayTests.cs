using System;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class TimeOfDayTests
    {
        [Test]
        public void GivenNullTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenNullTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenEmptyTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenEmptyTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenWhitespaceTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenWhitespaceTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenRandomTextTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenRandomTextTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenRandomTextWithColonTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenRandomTextWithColonTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenTooBigHourTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooBigHourTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenTooBigMinuteTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooBigMinuteTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenTooSmallHourTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooSmallHourTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenTooSmallMinuteTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenTooSmallMinuteTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenNonQuarterOfHourTimeString_WhenConstruct_ThenThrowInvalidTime()
        {
            var timeString = GivenNonQuarterOfHourTimeString();
            var respond = WhenConstruct(timeString);
            ThenThrowInvalidTimeOfDay(respond);
        }

        [Test]
        public void GivenValidTimeString_WhenConstruct_ThenCreateTimeOfDay()
        {
            var timeString = GivenValidTimeString();
            var respond = WhenConstruct(timeString);
            ThenCreateTimeOfDay(respond);
        }

        [Test]
        public void GivenMidnightTimeString_WhenConstruct_ThenCreateMidnightTimeOfDay()
        {
            var timeString = GivenMidnightTimeString();
            var respond = WhenConstruct(timeString);
            ThenCreateMidnightTimeOfDay(respond);
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
                return new TimeOfDay(timeString);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenThrowInvalidTimeOfDay(object response)
        {
            Assert.That(response, Is.InstanceOf<InvalidTimeOfDay>());
        }

        private void ThenCreateTimeOfDay(object response)
        {
            Assert.That(response, Is.InstanceOf<TimeOfDay>());
            var timeOfDay = (TimeOfDay)response;
            Assert.That(timeOfDay.Hour, Is.EqualTo(9));
            Assert.That(timeOfDay.Minute, Is.EqualTo(45));
        }

        private void ThenCreateMidnightTimeOfDay(object response)
        {
            Assert.That(response, Is.InstanceOf<TimeOfDay>());
            var timeOfDay = (TimeOfDay)response;
            Assert.That(timeOfDay.Hour, Is.EqualTo(0));
            Assert.That(timeOfDay.Minute, Is.EqualTo(0));
        }

        private void AssertIsAfter(string later, string earlier, bool isAfter)
        {
            var laterPit = new TimeOfDay(later);
            var earlierPit = new TimeOfDay(earlier);

            Assert.That(laterPit.IsAfter(earlierPit), Is.EqualTo(isAfter));
        }

        private void AssertToData(string time)
        {
            Assert.That(new TimeOfDay(time).ToData(), Is.EqualTo(time));
        }


        public class TimePoints
        {
            public TimeOfDay Earlier { get; set; }
            public TimeOfDay Later { get; set; }

            public TimePoints(string earlier, string later)
            {
                Earlier = new TimeOfDay(earlier);
                Later = new TimeOfDay(later);
            }
        }
    }
}
