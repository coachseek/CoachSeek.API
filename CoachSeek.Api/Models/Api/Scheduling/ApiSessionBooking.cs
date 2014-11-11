﻿using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Scheduling
{
    public class ApiSessionBooking
    {
        [Required]
        public int? StudentCapacity { get; set; }
        public bool IsOnlineBookable { get; set; } // eg. Is private or not
    }
}