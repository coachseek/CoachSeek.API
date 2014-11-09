﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiServiceKey
    {
        [Required]
        public Guid? Id { get; set; }

        public string Name { get; set; }
    }
}