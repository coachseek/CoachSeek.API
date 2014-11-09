using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api.Scheduling
{
    public class ApiSessionSaveCommand : ApiSaveCommand, IApiBusinessIdable
    {
        [Required]
        public Guid? BusinessId { get; set; }

        [Required]
        public ApiServiceKey Service { get; set; }
        [Required]
        public ApiLocationKey Location { get; set; }
        [Required]
        public ApiCoachKey Coach { get; set; }

        [Required]
        public string StartDate { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public int? Duration { get; set; }
        [Required]
        public int? StudentCapacity { get; set; }
        public bool IsOnlineBookable { get; set; } // eg. Is private or not
        public string Colour { get; set; }

        //public ApiSessionPricing Pricing { get; set; }
        //public ApiSessionRepetition Repetition { get; set; }
    }
}