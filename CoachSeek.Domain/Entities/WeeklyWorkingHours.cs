using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class WeeklyWorkingHours
    {
        public WeeklyWorkingHoursData WorkingHours { get; private set; }

        public WeeklyWorkingHours(WeeklyWorkingHoursData workingHoursData)
        {
            WorkingHours = workingHoursData;
        }
    }
}
