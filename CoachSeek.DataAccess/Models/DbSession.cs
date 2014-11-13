using System;

namespace CoachSeek.DataAccess.Models
{
    public class DbSession
    {
        public Guid Id { get; set; }
        public DbServiceKey Service { get; set; }
        public DbLocationKey Location { get; set; }
        public DbCoachKey Coach { get; set; }

        public DbSessionTiming Timing { get; set; }
        public DbSessionBooking Booking { get; set; }
        public DbPricing Pricing { get; set; }
        public DbRepetition Repetition { get; set; }
        public DbPresentation Presentation { get; set; }

    }
}
