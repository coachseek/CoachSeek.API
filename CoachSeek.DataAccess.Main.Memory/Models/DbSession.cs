using System;
using CoachSeek.DataAccess.Models;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public abstract class DbSession
    {
        public Guid Id { get; set; }
        public DbServiceKey Service { get; set; }
        public DbLocationKey Location { get; set; }
        public DbCoachKey Coach { get; set; }

        public DbSessionTiming Timing { get; set; }
        public DbSessionBooking Booking { get; set; }
        public DbPresentation Presentation { get; set; }
        public DbRepetition Repetition { get; set; }


        protected DbSession()
        {
            Service = new DbServiceKey();
            Location = new DbLocationKey();
            Coach = new DbCoachKey();
            Timing = new DbSessionTiming();
            Booking = new DbSessionBooking();
            Presentation = new DbPresentation();
            Repetition = new DbRepetition();
        }
    }
}
