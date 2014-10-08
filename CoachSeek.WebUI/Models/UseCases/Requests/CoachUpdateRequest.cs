﻿using System;

namespace CoachSeek.WebUI.Models.UseCases.Requests
{
    public class CoachUpdateRequest : Request
    {
        public Guid CoachId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}