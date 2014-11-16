using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewService : Service
    {
        public NewService(string name, 
                          string description, 
                          ServiceDefaultsData defaults,
                          ServiceBookingData booking,
                          PricingData pricing, 
                          RepetitionData repetition)
            : base(Guid.NewGuid(), name, description, defaults, booking, pricing, repetition)
        { }

        public NewService(NewServiceData data)
            : this(data.Name, data.Description, data.Defaults, data.Booking, data.Pricing, data.Repetition)
        { }
    }
}