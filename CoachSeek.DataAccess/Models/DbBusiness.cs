﻿using System;
using System.Collections.Generic;

namespace CoachSeek.DataAccess.Models
{
    public class DbBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public DbBusinessAdmin Admin { get; set; }
        public List<DbLocation> Locations { get; set; }
        public List<DbCoach> Coaches { get; set; }
    }
}