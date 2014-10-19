using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit
{
    public abstract class CoachUseCaseTests : UseCaseTests
    {
        protected WeeklyWorkingHoursCommand SetupStandardWorkingHoursCommand()
        {
            return new WeeklyWorkingHoursCommand
            {
                Monday = new DailyWorkingHoursCommand(true, "9:00", "17:00"),
                Tuesday = new DailyWorkingHoursCommand(true, "9:00", "17:00"),
                Wednesday = new DailyWorkingHoursCommand(true, "9:00", "17:00"),
                Thursday = new DailyWorkingHoursCommand(true, "9:00", "17:00"),
                Friday = new DailyWorkingHoursCommand(true, "9:00", "17:00"),
                Saturday = new DailyWorkingHoursCommand(false),
                Sunday = new DailyWorkingHoursCommand(false)
            };
        }

        protected WeeklyWorkingHoursCommand SetupWeekendWorkingHoursCommand()
        {
            return new WeeklyWorkingHoursCommand
            {
                Monday = new DailyWorkingHoursCommand(false),
                Tuesday = new DailyWorkingHoursCommand(false),
                Wednesday = new DailyWorkingHoursCommand(false),
                Thursday = new DailyWorkingHoursCommand(false),
                Friday = new DailyWorkingHoursCommand(false),
                Saturday = new DailyWorkingHoursCommand(true, "9:00", "17:00"),
                Sunday = new DailyWorkingHoursCommand(true, "9:00", "17:00")
            };
        }


        protected void AssertStandardWorkingHours(CoachData coach)
        {
            Assert.That(coach.WorkingHours, Is.Not.Null);
            AssertWorkingDay(coach.WorkingHours.Monday);
            AssertWorkingDay(coach.WorkingHours.Tuesday);
            AssertWorkingDay(coach.WorkingHours.Wednesday);
            AssertWorkingDay(coach.WorkingHours.Thursday);
            AssertWorkingDay(coach.WorkingHours.Friday);
            AssertNonWorkingDay(coach.WorkingHours.Saturday);
            AssertNonWorkingDay(coach.WorkingHours.Sunday);
        }

        protected void AssertWeekendWorkingHours(CoachData coach)
        {
            Assert.That(coach.WorkingHours, Is.Not.Null);
            AssertNonWorkingDay(coach.WorkingHours.Monday);
            AssertNonWorkingDay(coach.WorkingHours.Tuesday);
            AssertNonWorkingDay(coach.WorkingHours.Wednesday);
            AssertNonWorkingDay(coach.WorkingHours.Thursday);
            AssertNonWorkingDay(coach.WorkingHours.Friday);
            AssertWorkingDay(coach.WorkingHours.Saturday);
            AssertWorkingDay(coach.WorkingHours.Sunday);
        }

        private void AssertWorkingDay(DailyWorkingHoursData day)
        {
            Assert.That(day.IsAvailable, Is.True);
            Assert.That(day.StartTime, Is.EqualTo("9:00"));
            Assert.That(day.FinishTime, Is.EqualTo("17:00"));
        }

        private void AssertNonWorkingDay(DailyWorkingHoursData day)
        {
            Assert.That(day.IsAvailable, Is.False);
            Assert.That(day.StartTime, Is.Null);
            Assert.That(day.FinishTime, Is.Null);
        }
    }
}
