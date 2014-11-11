using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Scheduling
{
    public class ApiSessionTiming
    {
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string StartTime { get; set; }

        // Duration is not required because it may default to the Service value.
        public int? Duration { get; set; }
    }
}