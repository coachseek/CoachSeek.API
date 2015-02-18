using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class StandaloneSession : SingleSession
    {
        public StandaloneSession(SessionAddCommand command, LocationData location, CoachData coach, ServiceData service)
            : base(command, location, coach, service)
        {
            ParentId = null;
        }

        public StandaloneSession(SessionUpdateCommand command, LocationData location, CoachData coach, ServiceData service)
            : base(command, location, coach, service)
        {
            ParentId = null;
        }

        public StandaloneSession(SingleSessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Pricing)
        {
            // Fatal error! There is a defect in the system.
            if (data.ParentId.HasValue)
                throw new InvalidOperationException("We have a ParentId for a Standalone Session!");
        }

        public StandaloneSession(Guid id,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       SingleSessionPricingData pricing)
            : base(id, location, coach, service, timing, booking, presentation, pricing)
        { }


        protected override void ValidateAdditional(ValidationException errors,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionAddCommand command)
        {
            ValidateRepetition(command);
            ValidateAndCreateSessionPricing(command.Pricing, service.Pricing, errors);
        }


        private void ValidateRepetition(SessionAddCommand command)
        { }

        private void ValidateAndCreateSessionPricing(PricingCommand sessionPricing, SingleSessionPricingData servicePricing, ValidationException errors)
        {
            try
            {
                _pricing = new StandaloneSessionPricing(sessionPricing, servicePricing);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }
    }
}
