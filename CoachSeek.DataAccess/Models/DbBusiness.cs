using System;
using System.Collections.Generic;

namespace CoachSeek.DataAccess.Models
{
    public class DbBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }

        public List<DbLocation> Locations { get; set; }
        public List<DbCoach> Coaches { get; set; }
        public List<DbService> Services { get; set; }
        public List<DbSession> Sessions { get; set; }

        public DbBusiness()
        {
            Locations = new List<DbLocation>();
            Coaches = new List<DbCoach>();
            Services = new List<DbService>();
            Sessions = new List<DbSession>();
        }
    }
}