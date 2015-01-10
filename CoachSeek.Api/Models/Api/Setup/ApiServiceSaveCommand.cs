using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiServiceSaveCommand : ApiSaveCommand
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public ApiServiceTiming Timing { get; set; }
        public ApiServiceBooking Booking { get; set; }
        [Required]
        public ApiRepetition Repetition { get; set; }
        public ApiPricing Pricing { get; set; }
        public ApiPresentation Presentation { get; set; }
    }
}