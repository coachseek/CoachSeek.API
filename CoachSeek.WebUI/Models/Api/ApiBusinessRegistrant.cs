using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiBusinessRegistrant
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Password { get; set; }
    }
}