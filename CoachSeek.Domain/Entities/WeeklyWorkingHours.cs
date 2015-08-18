using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
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


        public WeeklyWorkingHours(WeeklyWorkingHoursCommand workingHoursCommand)
        {
            var errors = new ValidationException();

            _monday = CreateWorkingHours(workingHoursCommand.Monday, errors, "Monday");
            _tuesday = CreateWorkingHours(workingHoursCommand.Tuesday, errors, "Tuesday");
            _wednesday = CreateWorkingHours(workingHoursCommand.Wednesday, errors, "Wednesday");
            _thursday = CreateWorkingHours(workingHoursCommand.Thursday, errors, "Thursday");
            _friday = CreateWorkingHours(workingHoursCommand.Friday, errors, "Friday");
            _saturday = CreateWorkingHours(workingHoursCommand.Saturday, errors, "Saturday");
            _sunday = CreateWorkingHours(workingHoursCommand.Sunday, errors, "Sunday");

            errors.ThrowIfErrors();
        }

        public WeeklyWorkingHours(WeeklyWorkingHoursData workingHoursData)
        {
            _monday = new DailyWorkingHours(workingHoursData.Monday, "Monday");
            _tuesday = new DailyWorkingHours(workingHoursData.Tuesday, "Tuesday");
            _wednesday = new DailyWorkingHours(workingHoursData.Wednesday, "Wednesday");
            _thursday = new DailyWorkingHours(workingHoursData.Thursday, "Thursday");
            _friday = new DailyWorkingHours(workingHoursData.Friday, "Friday");
            _saturday = new DailyWorkingHours(workingHoursData.Saturday, "Saturday");
            _sunday = new DailyWorkingHours(workingHoursData.Sunday, "Sunday");
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


        private DailyWorkingHours CreateWorkingHours(DailyWorkingHoursCommand workingDayCommand, ValidationException errors, string dayOfWeek)
        {
            try
            {
                return new DailyWorkingHours(workingDayCommand, dayOfWeek);
            }
            catch (SingleErrorException ex)
            {
                errors.Add(ex);
                return null;
            }
        }
    }
}
