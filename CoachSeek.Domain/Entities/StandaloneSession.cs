using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class StandaloneSession : SingleSession
    {
        public StandaloneSession(SessionAddCommand command, LocationData location, CoachData coach, ServiceData service)
            : base(command, location, coach, service)
        { }

        public StandaloneSession(SessionUpdateCommand command, LocationData location, CoachData coach, ServiceData service)
            : base(command, location, coach, service)
        { }

        public StandaloneSession(SingleSessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Pricing)
        { }

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
            ValidateAndCreateSessionPricing(command.Pricing, service.Pricing, errors);
        }


        //public SingleSessionData ToData()
        //{
        //    return Mapper.Map<StandaloneSession, SingleSessionData>(this);
        //}


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

        private RepetitionData CreateSingleSessionRepetitionData()
        {
            return new RepetitionData(1);
        }
    }
}
