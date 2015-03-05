using System.ComponentModel.DataAnnotations;
using CoachSeek.Api.Models.Api.Setup;

namespace CoachSeek.Api.Models.Api.Scheduling
{
    public class ApiSessionSaveCommand : ApiSaveCommand
    {
        [Required]
        public ApiServiceKey Service { get; set; }
        [Required]
        public ApiLocationKey Location { get; set; }
        [Required]
        public ApiCoachKey Coach { get; set; }
        // Timing is required because we need the StartDate and StartTime.
        [Required]
        public ApiSessionTiming Timing { get; set; }
        // Booking is not required because it may default to the Service value.
        [Required]
        public ApiSessionBooking Booking { get; set; }
        // Pricing is not required because it may default to the Service value.
        [Required]
        public ApiPricing Pricing { get; set; }
        // Repetition is not required because it may default to the Service value.
        [Required]
        public ApiRepetition Repetition { get; set; }
        // Presentation is not required because it may default to the Service value.
        [Required]
        public ApiPresentation Presentation { get; set; }
    }
}