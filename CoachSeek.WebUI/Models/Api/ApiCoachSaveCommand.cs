﻿using System;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiCoachSaveCommand
    {
        public Guid BusinessId { get; set; }
        public Guid? Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email{ get; set; }
        public string Phone { get; set; }


        public bool IsNew()
        {
            return !Id.HasValue;
        }
    }
}