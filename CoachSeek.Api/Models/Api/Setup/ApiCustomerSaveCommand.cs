using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiCustomerSaveCommand : ApiSaveCommand
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [StringLength(100), EmailAddress]
        public string Email { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
    }
}