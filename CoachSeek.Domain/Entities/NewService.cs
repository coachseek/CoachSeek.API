using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewService : Service
    {
        public NewService(string name, 
                          string description,
                          ServiceTimingData timing,
                          ServiceBookingData booking,
                          PricingData pricing,
                          RepetitionData repetition,
                          PresentationData presentation)
            : base(Guid.NewGuid(), name, description, timing, booking, pricing, repetition, presentation)
        { }

        public NewService(NewServiceData data)
            : this(data.Name, data.Description, data.Timing, data.Booking, data.Pricing, data.Repetition, data.Presentation)
        { }
    }
}