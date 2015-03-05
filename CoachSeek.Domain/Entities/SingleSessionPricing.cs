using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SingleSessionPricing
    {
        protected Price _sessionPrice;

        public decimal? SessionPrice
        {
            get { return _sessionPrice == null ? null : _sessionPrice.Amount; }
        }


        public SingleSessionPricing()
        { }

        public SingleSessionPricing(PricingCommand sessionPricing)
        {
            Validate(sessionPricing);

            CreateSessionPrice(sessionPricing.SessionPrice);
        }

        public SingleSessionPricing(decimal? sessionPrice)
        {
            CreateSessionPrice(sessionPrice);
        }


        public SingleSessionPricingData ToData()
        {
            return Mapper.Map<SingleSessionPricing, SingleSessionPricingData>(this);
        }


        private void Validate(PricingCommand pricing)
        {
            var errors = new ValidationException();

            ValidateSessionPrice(errors, pricing);
            ValidateAdditional(errors, pricing);

            errors.ThrowIfErrors();
        }

        private void CreateSessionPrice(decimal? sessionPrice)
        {
            _sessionPrice = new Price(sessionPrice);
        }

        private static void ValidateSessionPrice(ValidationException errors, PricingCommand pricing)
        {
            try
            {
                var sessionPrice = new Price(pricing.SessionPrice);
            }
            catch (InvalidPrice)
            {
                errors.Add("The sessionPrice field is not valid.", "session.pricing.sessionPrice");
            }
        }

        protected virtual void ValidateAdditional(ValidationException errors, PricingCommand pricing)
        {
            // Note: For a single session inside of a repeated sesssion (course) 
            // is's ok for the session not to have a session price.
            // On the other hand, a (child class) Standalone session must have a session price.
        }
    }
}
