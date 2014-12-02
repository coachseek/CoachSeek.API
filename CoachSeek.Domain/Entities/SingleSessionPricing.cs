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

            ValidateAndCreateSingleSessionPricing(sessionPricing);
        }

        public SingleSessionPricing(decimal? sessionPrice)
        {
            _sessionPrice = new Price(sessionPrice);
        }


        public PricingData ToData()
        {
            return Mapper.Map<SingleSessionPricing, PricingData>(this);
        }


        private void ValidateAndCreateSingleSessionPricing(PricingData pricing)
        {
            if (pricing == null)
            {
                _sessionPrice = new Price();
                return;
            }

            var errors = new ValidationException();

            try
            {
                _sessionPrice = new Price(pricing.SessionPrice);
            }
            catch (InvalidPrice)
            {
                errors.Add("The sessionPrice field is not valid.", "session.pricing.sessionPrice");
            }

            if (pricing.CoursePrice.HasValue)
                errors.Add("The coursePrice field must not be specified for a single session.", "session.pricing.coursePrice");

            errors.ThrowIfErrors();
        }

        private PricingData BackfillMissingValuesFromService(PricingData sessionPricing, PricingData servicePricing)
        {
            if (sessionPricing == null)
                return servicePricing;

            if (servicePricing != null && !sessionPricing.SessionPrice.HasValue)
                sessionPricing.SessionPrice = servicePricing.SessionPrice;

            return sessionPricing;
        }
    }
}
