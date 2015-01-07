using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiCoachSaveCommand : ApiSaveCommand
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public ApiWeeklyWorkingHours WorkingHours { get; set; }
    }
}