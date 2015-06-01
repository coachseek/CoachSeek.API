using System;
using System.Collections.Generic;
using CoachSeek.DataAccess.Models;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Currency { get; set; }
        public string PaymentProvider { get; set; }
        public string MerchantAccountIdentifier { get; set; }

        public List<DbLocation> Locations { get; set; }
        public List<DbCoach> Coaches { get; set; }
        public List<DbService> Services { get; set; }
        public List<DbSingleSession> Sessions { get; set; }
        public List<DbRepeatedSession> Courses { get; set; }
        public List<DbCustomer> Customers { get; set; }

        public DbBusiness()
        {
            Locations = new List<DbLocation>();
            Coaches = new List<DbCoach>();
            Services = new List<DbService>();
            Sessions = new List<DbSingleSession>();
            Courses = new List<DbRepeatedSession>();
            Customers = new List<DbCustomer>();
        }
    }
}