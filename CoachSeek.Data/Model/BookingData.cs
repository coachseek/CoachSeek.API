using System;

namespace CoachSeek.Data.Model
{
    public class BookingData
    {
        public Guid Id { get; set; }
        public SessionKeyData Session { get; set; }
        public CustomerKeyData Customer { get; set; }
    }
}