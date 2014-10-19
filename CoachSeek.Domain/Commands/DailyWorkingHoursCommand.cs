namespace CoachSeek.Domain.Commands
{
    public class DailyWorkingHoursCommand
    {
        public bool IsAvailable { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }

        public DailyWorkingHoursCommand()
        { }

        public DailyWorkingHoursCommand(bool isAvailable, string startTime = null, string finishTime = null)
        {
            IsAvailable = isAvailable;
            StartTime = startTime;
            FinishTime = finishTime;
        }
    }
}
