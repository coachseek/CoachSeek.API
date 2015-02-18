namespace CoachSeek.Domain.Commands
{
    public class RepetitionCommand
    {
        public int SessionCount { get; set; }
        public string RepeatFrequency { get; set; }


        public RepetitionCommand()
        { }

        public RepetitionCommand(int sessionCount, string repeatFrequency = null)
        {
            SessionCount = sessionCount;
            RepeatFrequency = repeatFrequency;
        }
    }
}
