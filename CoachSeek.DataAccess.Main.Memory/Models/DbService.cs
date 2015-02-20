using System;

namespace CoachSeek.DataAccess.Models
{
    public class DbService
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DbServiceTiming Timing { get; set; }
        public DbServiceBooking Booking { get; set; }
        public DbRepeatedSessionPricing Pricing { get; set; }
        public DbRepetition Repetition { get; set; }
        public DbPresentation Presentation { get; set; }
    }
}
