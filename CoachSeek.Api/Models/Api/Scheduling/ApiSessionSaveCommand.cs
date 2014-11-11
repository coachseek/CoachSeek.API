using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Scheduling
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
        public ApiSessionTiming Timing { get; set; }
        [Required]
        public ApiSessionBooking Booking { get; set; }
        public ApiPresentation Presentation { get; set; }
    }
}