using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class Session
    {
        private Service _service;
        private Location _location;
        private Coach _coach;
        private SessionTiming _timing;
        private Presentation _presentation;


        public Guid Id { get; private set; }
        public ServiceKeyData Service { get { return _service.ToKeyData(); } }
        public LocationKeyData Location { get { return _location.ToKeyData(); } }
        public CoachKeyData Coach { get { return _coach.ToKeyData(); } }
        public SessionTimingData Timing { get { return _timing.ToData(); } }
        public SessionBookingData Booking { get; set; }
        public PricingData Pricing { get; set; }
        public RepetitionData Repetition { get; set; }
        public PresentationData Presentation { get { return _presentation.ToData(); } }


        public Session(SessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Pricing, data.Repetition, data.Presentation)
        { }

        public Session(Guid id, 
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PricingData pricing,
                       RepetitionData repetition,
                       PresentationData presentation)
        {
            Id = id;
            _location = new Location(location);
            _coach = new Coach(coach);
            _service = new Service(service);

            _timing = new SessionTiming(timing);

            _presentation = new Presentation(presentation);
        }

        public SessionData ToData()
        {
            return Mapper.Map<Session, SessionData>(this);
        }
    }
}
