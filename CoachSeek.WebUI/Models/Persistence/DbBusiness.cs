using System;
using System.Collections.Generic;

namespace CoachSeek.WebUI.Models.Persistence
{
    public class DbBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public DbBusinessAdmin Admin { get; set; }
        public List<DbLocation> Locations { get; set; }
    }
}