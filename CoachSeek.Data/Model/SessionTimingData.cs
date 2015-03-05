namespace CoachSeek.Data.Model
{
    public class SessionTimingData
    {
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public int Duration { get; set; }


        public SessionTimingData()
        { }

        public SessionTimingData(string startDate, string startTime, int duration)
        {
            StartDate = startDate;
            StartTime = startTime;
            Duration = duration;
        }
    }
}
