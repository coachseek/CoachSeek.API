namespace CoachSeek.DataAccess.Models
{
    public class DbDailyWorkingHours
    {
        public bool IsAvailable { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }

        public DbDailyWorkingHours()
        { }

        public DbDailyWorkingHours(bool isAvailable, string startTime = null, string finishTime = null)
        {
            IsAvailable = isAvailable;
            StartTime = startTime;
            FinishTime = finishTime;
        }
    }
}
