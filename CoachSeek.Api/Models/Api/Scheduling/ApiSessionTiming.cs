using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Scheduling
{
    public class ApiSessionTiming
    {
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public int? Duration { get; set; }
        public int? BookUntil { get; set; }
    }
}