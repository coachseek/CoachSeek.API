using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Services;

namespace CoachSeek.Domain.Entities
{
    public class BookingSession
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        private int StudentCapacity { get; set; }
        private int BookingCount { get; set; }
        public Date Date { get; private set; }
        public TimeOfDay StartTime { get; private set; }

        public BookingSession(SingleSessionData data)
        {
            Id = data.Id;
            Name = SessionNamer.Name(data);
            StudentCapacity = data.Booking.StudentCapacity;
            BookingCount = data.Booking.BookingCount;
            Date = new Date(data.Timing.StartDate);
            StartTime = new TimeOfDay(data.Timing.StartTime);
        }

        public BookingSession(BookingSessionData data)
        {
            Id = data.Id;
            Name = data.Name;
            StudentCapacity = data.StudentCapacity;
            BookingCount = data.BookingCount;
            Date = new Date(data.Date);
            StartTime = new TimeOfDay(data.StartTime);
        }

        public bool IsFull
        {
            get { return BookingCount >= StudentCapacity; }
        }

        public BookingSessionData ToData()
        {
            return new BookingSessionData
            {
                Id = Id,
                Name = Name,
                StudentCapacity = StudentCapacity,
                BookingCount = BookingCount,
                Date = Date.ToString(),
                StartTime = StartTime.ToString()
            };
        }
    }
}
