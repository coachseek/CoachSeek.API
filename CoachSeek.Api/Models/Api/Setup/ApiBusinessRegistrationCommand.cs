using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiBusinessRegistrationCommand
    {
        [Required]
        public ApiBusinessCommand Business { get; set; }

        [Required]
        public ApiBusinessAdminCommand Admin { get; set; }
    }
}