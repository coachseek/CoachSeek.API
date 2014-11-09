using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api.Setup
{
    public class ApiCoachSaveCommand : ApiSaveCommand, IApiBusinessIdable
    {
        [Required]
        public Guid? BusinessId { get; set; }

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