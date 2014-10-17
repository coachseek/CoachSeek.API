namespace CoachSeek.Domain.Commands
{
    public class WeeklyWorkingHoursCommand
    {
        public DailyWorkingHoursCommand Monday { get; set; }
        public DailyWorkingHoursCommand Tuesday { get; set; }
        public DailyWorkingHoursCommand Wednesday { get; set; }
        public DailyWorkingHoursCommand Thursday { get; set; }
        public DailyWorkingHoursCommand Friday { get; set; }
        public DailyWorkingHoursCommand Saturday { get; set; }
        public DailyWorkingHoursCommand Sunday { get; set; }
    }
}
