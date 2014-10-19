using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class WeeklyWorkingHours
    {
        public DailyWorkingHoursData Monday { get { return WorkingHours.Monday; } }
        public DailyWorkingHoursData Tuesday { get { return WorkingHours.Tuesday; } }
        public DailyWorkingHoursData Wednesday { get { return WorkingHours.Wednesday; } }
        public DailyWorkingHoursData Thursday { get { return WorkingHours.Thursday; } }
        public DailyWorkingHoursData Friday { get { return WorkingHours.Friday; } }
        public DailyWorkingHoursData Saturday { get { return WorkingHours.Saturday; } }
        public DailyWorkingHoursData Sunday { get { return WorkingHours.Sunday; } }


        public WeeklyWorkingHoursData WorkingHours { get; private set; }

        public WeeklyWorkingHours(WeeklyWorkingHoursData workingHoursData)
        {
            WorkingHours = workingHoursData;
        }

        public WeeklyWorkingHoursData ToData()
        {
            return WorkingHours;
        }
    }
}
