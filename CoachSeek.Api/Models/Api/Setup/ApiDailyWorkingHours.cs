namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiDailyWorkingHours
    {
        public bool IsAvailable { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }

        public ApiDailyWorkingHours()
        { }

        public ApiDailyWorkingHours(bool isAvailable, string startTime, string finishTime)
        {
            IsAvailable = isAvailable;
            StartTime = startTime;
            FinishTime = finishTime;
        }
    }
}