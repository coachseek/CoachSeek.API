using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiBusinessRegistrationCommand
    {
        [Required, StringLength(100)]
        public string BusinessName { get; set; }

        [Required]
        public ApiBusinessRegistrant Registrant { get; set; }
    }
}