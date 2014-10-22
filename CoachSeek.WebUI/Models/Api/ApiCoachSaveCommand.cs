using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiCoachSaveCommand : IApiBusinessIdable, IApiIdable
    {
        [Required]
        public Guid? BusinessId { get; set; }
        public Guid? Id { get; set; }

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


        public bool IsNew()
        {
            return !Id.HasValue;
        }
    }
}