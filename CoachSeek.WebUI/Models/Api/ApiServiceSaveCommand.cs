using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiServiceSaveCommand : IApiBusinessIdable, IApiIdable
    {
        [Required]
        public Guid? BusinessId { get; set; }
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public ApiServiceDefaults Defaults { get; set; }
            

        public bool IsNew()
        {
            return !Id.HasValue;
        }
    }
}