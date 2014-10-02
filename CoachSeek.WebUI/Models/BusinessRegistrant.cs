using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models
{
    public class BusinessRegistrant
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        
        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Password { get; set; }
    }
}