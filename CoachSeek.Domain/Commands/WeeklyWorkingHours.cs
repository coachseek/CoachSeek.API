namespace CoachSeek.Domain.Commands
{
    public class WeeklyWorkingHours
    {
        public DailyWorkingHours Monday { get; set; }
        public DailyWorkingHours Tuesday { get; set; }
        public DailyWorkingHours Wednesday { get; set; }
        public DailyWorkingHours Thursday { get; set; }
        public DailyWorkingHours Friday { get; set; }
        public DailyWorkingHours Saturday { get; set; }
        public DailyWorkingHours Sunday { get; set; }
    }
}
