using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiCustomerSaveCommand : ApiSaveCommand
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string Phone { get; set; }
    }
}