using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class WeeklyWorkingHours
    {
        private DailyWorkingHours _monday;
        private DailyWorkingHours _tuesday;
        private DailyWorkingHours _wednesday;
        private DailyWorkingHours _thursday;
        private DailyWorkingHours _friday;
        private DailyWorkingHours _saturday;
        private DailyWorkingHours _sunday;


        public DailyWorkingHoursData Monday { get { return _monday.ToData(); } }
        public DailyWorkingHoursData Tuesday { get { return _tuesday.ToData(); } }
        public DailyWorkingHoursData Wednesday { get { return _wednesday.ToData(); } }
        public DailyWorkingHoursData Thursday { get { return _thursday.ToData(); } }
        public DailyWorkingHoursData Friday { get { return _friday.ToData(); } }
        public DailyWorkingHoursData Saturday { get { return _saturday.ToData(); } }
        public DailyWorkingHoursData Sunday { get { return _sunday.ToData(); } }


        public WeeklyWorkingHours(WeeklyWorkingHoursData workingHoursData)
        {
            var errors = new ValidationException();

            _monday = CreateWorkingHours(workingHoursData.Monday, errors, "monday");
            _tuesday = CreateWorkingHours(workingHoursData.Tuesday, errors, "tuesday");
            _wednesday = CreateWorkingHours(workingHoursData.Wednesday, errors, "wednesday");
            _thursday = CreateWorkingHours(workingHoursData.Thursday, errors, "thursday");
            _friday = CreateWorkingHours(workingHoursData.Friday, errors, "friday");
            _saturday = CreateWorkingHours(workingHoursData.Saturday, errors, "saturday");
            _sunday = CreateWorkingHours(workingHoursData.Sunday, errors, "sunday");

            errors.ThrowIfErrors();
        }

        private DailyWorkingHours CreateWorkingHours(DailyWorkingHoursData workingDay, ValidationException errors, string day)
        {
            try
            {
                return new DailyWorkingHours(workingDay);
            }
            catch (Exception)
            {
                errors.Add(string.Format("The {0} working hours are not valid.", day), string.Format("coach.workingHours.{0}", day));
                
                return null;
            }
        }

        public WeeklyWorkingHoursData ToData()
        {
            return new WeeklyWorkingHoursData
            {
                Monday = Monday,
                Tuesday = Tuesday,
                Wednesday = Wednesday,
                Thursday = Thursday,
                Friday = Friday,
                Saturday = Saturday,
                Sunday = Sunday
            };
        }
    }
}
