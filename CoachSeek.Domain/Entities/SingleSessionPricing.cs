using AutoMapper;
using CoachSeek.Data.Model;
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


        public SingleSessionPricing(PricingData sessionPricing, PricingData servicePricing)
        {
            sessionPricing = BackfillMissingValuesFromService(sessionPricing, servicePricing);

            Validate(sessionPricing);

            _sessionPrice = new Price(sessionPricing.SessionPrice);
        }

        public SingleSessionPricing(decimal? sessionPrice)
        {
            _sessionPrice = new Price(sessionPrice);
        }


        public PricingData ToData()
        {
            return Mapper.Map<SingleSessionPricing, PricingData>(this);
        }


        private void Validate(PricingData pricing)
        {
            var errors = new ValidationException();

            ValidateSessionPrice(errors, pricing);

            ValidateAdditional(errors, pricing);

            errors.ThrowIfErrors();
        }

        private static void ValidateSessionPrice(ValidationException errors, PricingData pricing)
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

        protected virtual void ValidateAdditional(ValidationException errors, PricingData pricing)
        { }

        protected PricingData BackfillMissingValuesFromService(PricingData sessionPricing, PricingData servicePricing)
        {
            if (sessionPricing == null)
                return servicePricing ?? new PricingData();

            if (servicePricing != null && !sessionPricing.SessionPrice.HasValue)
                sessionPricing.SessionPrice = servicePricing.SessionPrice;

            return sessionPricing;
        }
    }
}
