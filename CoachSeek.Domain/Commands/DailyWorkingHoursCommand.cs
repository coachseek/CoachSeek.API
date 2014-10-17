namespace CoachSeek.Domain.Commands
{
    public class DailyWorkingHoursCommand
    {
        public bool IsAvailable { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
    }
}
