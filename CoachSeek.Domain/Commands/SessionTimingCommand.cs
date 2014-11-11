namespace CoachSeek.Domain.Commands
{
    public class SessionTimingCommand
    {
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public int? Duration { get; set; }
    }
}
