﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
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