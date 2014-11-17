namespace CoachSeek.Data.Model
{
    public class RepetitionData
    {
        public int RepeatTimes { get; set; }
        public string RepeatFrequency { get; set; }


        public RepetitionData()
        { }

        public RepetitionData(int repeatTimes, string repeatFrequency = null)
        {
            RepeatTimes = repeatTimes;
            RepeatFrequency = repeatFrequency;
        }
    }
}
