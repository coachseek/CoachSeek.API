using System;

namespace CoachSeek.DataAccess.Models
{
    public class DbService
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DbServiceDefaults Defaults { get; set; }

        public DbServiceBooking Booking { get; set; }
        public DbPricing Pricing { get; set; }
        public DbRepetition Repetition { get; set; }
    }
}
