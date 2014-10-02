using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Requests
{
    public class BusinessRegistrationRequest
    {
        [Required, StringLength(100)]
        public string BusinessName { get; set; }

        [Required]
        public BusinessRegistrant Registrant { get; set; }
    }
}