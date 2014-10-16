﻿using System;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiLocationSaveCommand
    {
        public Guid BusinessId { get; set; }
        public Guid? Id { get; set; }

        public string Name { get; set; }


        public bool IsNew()
        {
            return !Id.HasValue;
        }
    }
}