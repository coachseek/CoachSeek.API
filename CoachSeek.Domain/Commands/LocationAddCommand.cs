﻿using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class LocationAddCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public string LocationName { get; set; }


        public NewLocationData ToData()
        {
            return new NewLocationData
            {
                Name = LocationName
            };
        }
    }
}