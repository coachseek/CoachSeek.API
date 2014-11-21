namespace CoachSeek.Data.Model
{
    public class RepetitionData
    {
        public int SessionCount { get; set; }
        public string RepeatFrequency { get; set; }


        public RepetitionData()
        { }

        public RepetitionData(int sessionCount, string repeatFrequency = null)
        {
            SessionCount = sessionCount;
            RepeatFrequency = repeatFrequency;
        }
    }
}
