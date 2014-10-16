﻿using System.Dynamic;

namespace CoachSeek.Data.Model
{
    public class NewCoachData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public WeeklyWorkingHoursData WorkingHours { get; set; }
    }
}