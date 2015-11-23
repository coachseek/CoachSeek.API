using System;

namespace CoachSeek.Data.Model
{
    public class BookingSessionData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StudentCapacity { get; set; }
        public int BookingCount { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
    }
}
