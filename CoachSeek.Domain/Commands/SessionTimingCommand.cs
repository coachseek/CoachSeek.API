namespace CoachSeek.Domain.Commands
{
    public class SessionTimingCommand
    {
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public int Duration { get; set; }
        public int BookUntil { get; set; }

        public SessionTimingCommand()
        { }

        public SessionTimingCommand(string startDate, string startTime, int duration, int bookUntil)
        {
            StartDate = startDate;
            StartTime = startTime;
            Duration = duration;
            BookUntil = bookUntil;
        }
    }
}
