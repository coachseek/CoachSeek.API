using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewSession : Session
    {
        public NewSession(LocationData location,
                          CoachData coach,
                          ServiceData service, 
                          SessionTimingData timing,
                          SessionBookingData booking,
                          PricingData pricing, 
                          RepetitionData repetition,
                          PresentationData presentation)
            : base(Guid.NewGuid(), location, coach, service, timing, booking, pricing, repetition, presentation)
        { }

        public NewSession(NewSessionData data, ServiceData service, LocationData location, CoachData coach)
            : this(location, coach, service, data.Timing, data.Booking, data.Pricing, data.Repetition, data.Presentation)
        { }
    }
}