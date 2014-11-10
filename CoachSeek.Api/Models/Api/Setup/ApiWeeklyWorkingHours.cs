using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiWeeklyWorkingHours
    {
        [Required]
        public ApiDailyWorkingHours Monday { get; set; }
        [Required]
        public ApiDailyWorkingHours Tuesday { get; set; }
        [Required]
        public ApiDailyWorkingHours Wednesday { get; set; }
        [Required]
        public ApiDailyWorkingHours Thursday { get; set; }
        [Required]
        public ApiDailyWorkingHours Friday { get; set; }
        [Required]
        public ApiDailyWorkingHours Saturday { get; set; }
        [Required]
        public ApiDailyWorkingHours Sunday { get; set; }
    }
}