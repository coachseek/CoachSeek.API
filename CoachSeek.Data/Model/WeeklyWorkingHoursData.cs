namespace CoachSeek.Data.Model
{
    public class WeeklyWorkingHoursData
    {
        public DailyWorkingHoursData Monday { get; set; }
        public DailyWorkingHoursData Tuesday { get; set; }
        public DailyWorkingHoursData Wednesday { get; set; }
        public DailyWorkingHoursData Thursday { get; set; }
        public DailyWorkingHoursData Friday { get; set; }
        public DailyWorkingHoursData Saturday { get; set; }
        public DailyWorkingHoursData Sunday { get; set; }
    }
}
