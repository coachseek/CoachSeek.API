using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Booking
{
    public class ApiBookingSaveCommand : ApiSaveCommand
    {
        [Required]
        public ApiSessionKey Session { get; set; }
        [Required]
        public ApiCustomerKey Customer { get; set; }

        public string PaymentStatus { get; set; }
        public bool? HasAttended { get; set; }
    }
}