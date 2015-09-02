﻿using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiBusinessCommand
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(3)]
        public string Currency { get; set; }

        [StringLength(100)]
        public string Sport { get; set; }
    }
}