using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class StandaloneSession : SingleSession
    {
        public RepetitionData Repetition { get { return CreateSingleSessionRepetitionData(); } }


        public StandaloneSession(SessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Pricing)
        { }

        public StandaloneSession(Guid id,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       PricingData pricing)
            : base(id, location, coach, service, timing, booking, presentation, pricing)
        {
            var errors = new ValidationException();

            ValidateAndCreateSessionPricing(pricing, service.Pricing, errors);

            errors.ThrowIfErrors();
        }


        public override SessionData ToData()
        {
            return Mapper.Map<StandaloneSession, SessionData>(this);
        }


        private void ValidateAndCreateSessionPricing(PricingData sessionPricing, PricingData servicePricing, ValidationException errors)
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
