namespace CoachSeek.Data.Model
{
    public class DailyWorkingHoursData
    {
        public bool IsAvailable { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }

        public DailyWorkingHoursData()
        { }

        public DailyWorkingHoursData(bool isAvailable, string startTime = null, string finishTime = null)
        {
            IsAvailable = isAvailable;
            StartTime = startTime;
            FinishTime = finishTime;
        }
    }
}
