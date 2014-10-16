namespace CoachSeek.WebUI.Models.Api
{
    public class ApiWeeklyWorkingHours
    {
        public ApiDailyWorkingHours Monday { get; set; }
        public ApiDailyWorkingHours Tuesday { get; set; }
        public ApiDailyWorkingHours Wednesday { get; set; }
        public ApiDailyWorkingHours Thursday { get; set; }
        public ApiDailyWorkingHours Friday { get; set; }
        public ApiDailyWorkingHours Saturday { get; set; }
        public ApiDailyWorkingHours Sunday { get; set; }
    }
}