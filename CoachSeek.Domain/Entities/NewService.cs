using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class NewService : Service
    {
        public NewService(string name,
                          string description,
                          ServiceTimingData timing,
                          ServiceBookingData booking,
                          RepeatedSessionPricingData pricing,
                          RepetitionData repetition,
                          PresentationData presentation)
            : base(Guid.NewGuid(), name, description, timing, booking, pricing, repetition, presentation)
        { }

        public NewService(string name,
                          string description,
                          ServiceTimingCommand timing,
                          ServiceBookingCommand booking,
                          PricingCommand pricing,
                          RepetitionCommand repetition,
                          PresentationCommand presentation)
            : base(Guid.NewGuid(), name, description, timing, booking, pricing, repetition, presentation)
        { }

        public NewService(NewServiceData data)
            : this(data.Name, data.Description, data.Timing, data.Booking, data.Pricing, data.Repetition, data.Presentation)
        { }

        public NewService(ServiceAddCommand command)
            : this(command.Name, command.Description, command.Timing, command.Booking, command.Pricing, command.Repetition, command.Presentation)
        { }
    }
}