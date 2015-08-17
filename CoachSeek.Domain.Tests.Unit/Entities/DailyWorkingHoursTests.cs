using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class DailyWorkingHoursTests
    {
        [Test]
        public void GivenInvalidStartTime_WhenConstruct_ThenThrowInvalidDailyWorkingHours()
        {
            var parameters = GivenInvalidStartTime();
            var response = WhenConstruct(parameters);
            ThenThrowInvalidDailyWorkingHours(response);
        }

        [Test]
        public void GivenInvalidFinishTime_WhenConstruct_ThenThrowInvalidDailyWorkingHours()
        {
            var parameters = GivenInvalidFinishTime();
            var response = WhenConstruct(parameters);
            ThenThrowInvalidDailyWorkingHours(response);
        }

        [Test]
        public void GivenIsAvailableButNoStartTime_WhenConstruct_ThenThrowInvalidDailyWorkingHours()
        {
            var parameters = GivenIsAvailableButNoStartTime();
            var response = WhenConstruct(parameters);
            ThenThrowInvalidDailyWorkingHours(response);
        }

        [Test]
        public void GivenIsAvailableButNoFinishTime_WhenConstruct_ThenThrowInvalidDailyWorkingHours()
        {
            var parameters = GivenIsAvailableButNoFinishTime();
            var response = WhenConstruct(parameters);
            ThenThrowInvalidDailyWorkingHours(response);
        }

        [Test]
        public void GivenIsAvailableButFinishTimeIsBeforeStartTime_WhenConstruct_ThenThrowInvalidDailyWorkingHours()
        {
            var parameters = GivenIsAvailableButFinishTimeIsBeforeStartTime();
            var response = WhenConstruct(parameters);
            ThenThrowInvalidDailyWorkingHours(response);
        }

        [Test]
        public void GivenIsAvailableButFinishTimeIsSameAsStartTime_WhenConstruct_ThenThrowInvalidDailyWorkingHours()
        {
            var parameters = GivenIsAvailableButFinishTimeIsSameAsStartTime();
            var response = WhenConstruct(parameters);
            ThenThrowInvalidDailyWorkingHours(response);
        }

        [Test]
        public void GivenIsAvailableAndHaveStartAndFinishTimes_WhenConstruct_ThenCreateAvailableDailyWorkingHours()
        {
            var parameters = GivenIsAvailableAndHaveStartAndFinishTimes();
            var response = WhenConstruct(parameters);
            ThenCreateAvailableDailyWorkingHours(response);
        }

        [Test]
        public void GivenIsNotAvailableAndHaveStartAndFinishTimes_WhenConstruct_ThenCreateUnavailableDailyWorkingHours()
        {
            var parameters = GivenIsNotAvailableAndHaveStartAndFinishTimes();
            var response = WhenConstruct(parameters);
            ThenCreateUnavailableDailyWorkingHours(response);
        }


        private DailyWorkingHoursData GivenInvalidStartTime()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = true,
                StartTime = "-4",
                FinishTime = "19:00"
            };
        }

        private DailyWorkingHoursData GivenInvalidFinishTime()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = true,
                StartTime = "16:00",
                FinishTime = "hello world!"
            };
        }

        private DailyWorkingHoursData GivenIsAvailableButNoStartTime()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = true,
                StartTime = null,
                FinishTime = "17:00"
            };
        }

        private DailyWorkingHoursData GivenIsAvailableButNoFinishTime()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = true,
                StartTime = "9:00",
                FinishTime = null
            };
        }

        private DailyWorkingHoursData GivenIsAvailableButFinishTimeIsBeforeStartTime()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = true,
                StartTime = "11:45",
                FinishTime = "8:15"
            };
        }

        private DailyWorkingHoursData GivenIsAvailableButFinishTimeIsSameAsStartTime()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = true,
                StartTime = "14:30",
                FinishTime = "14:30"
            };
        }

        private DailyWorkingHoursData GivenIsAvailableAndHaveStartAndFinishTimes()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = true,
                StartTime = "8:30",
                FinishTime = "17:45"
            };
        }

        private DailyWorkingHoursData GivenIsNotAvailableAndHaveStartAndFinishTimes()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = false,
                StartTime = "11:15",
                FinishTime = "23:45"
            };
        }


        private object WhenConstruct(DailyWorkingHoursData data)
        {
            try
            {
                return new DailyWorkingHours(data, "monday");
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenThrowInvalidDailyWorkingHours(object response)
        {
            Assert.That(response, Is.InstanceOf<DailyWorkingHoursInvalid>());
        }

        private void ThenCreateAvailableDailyWorkingHours(object response)
        {
            Assert.That(response, Is.InstanceOf<DailyWorkingHours>());
            var workingHours = (DailyWorkingHours)response;
            Assert.That(workingHours.IsAvailable, Is.True);
            Assert.That(workingHours.StartTime, Is.EqualTo("8:30"));
            Assert.That(workingHours.FinishTime, Is.EqualTo("17:45"));
        }

        private void ThenCreateUnavailableDailyWorkingHours(object response)
        {
            Assert.That(response, Is.InstanceOf<DailyWorkingHours>());
            var workingHours = (DailyWorkingHours)response;
            Assert.That(workingHours.IsAvailable, Is.False);
            Assert.That(workingHours.StartTime, Is.EqualTo("11:15"));
            Assert.That(workingHours.FinishTime, Is.EqualTo("23:45"));
        }
    }
}
