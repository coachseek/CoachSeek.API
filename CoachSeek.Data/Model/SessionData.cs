using System;
using AutoMapper;

namespace CoachSeek.Data.Model
{
    public abstract class SessionData
    {
        public Guid Id { get; set; }

        public LocationKeyData Location { get; set; }
        public CoachKeyData Coach { get; set; }
        public ServiceKeyData Service { get; set; }

        public SessionTimingData Timing { get; set; }
        public SessionBookingData Booking { get; set; }
        public PresentationData Presentation { get; set; }


        protected SessionData()
        { }

        protected SessionData(SessionData session)
        {
            Id = session.Id;
            Location = session.Location;
            Coach = session.Coach;
            Service = session.Service;
            Timing = session.Timing;
            Booking = session.Booking;
            Presentation = session.Presentation;
        }


        public SessionKeyData ToKeyData()
        {
            return Mapper.Map<SessionData, SessionKeyData>(this);
        }
    }
}
