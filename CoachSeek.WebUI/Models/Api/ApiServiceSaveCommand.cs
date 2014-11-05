using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiServiceSaveCommand : ApiSaveCommand, IApiBusinessIdable
    {
        [Required]
        public Guid? BusinessId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public ApiServiceDefaults Defaults { get; set; }
        public ApiServicePricing Pricing { get; set; }
        public ApiServiceRepetition Repetition { get; set; }
    }
}