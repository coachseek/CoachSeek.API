using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class StandaloneSessionPricing : SingleSessionPricing
    {
        public new decimal SessionPrice { get { return _sessionPrice.Amount.Value; } }


        public StandaloneSessionPricing(PricingData sessionPricing, PricingData servicePricing)
            : base(sessionPricing, servicePricing)
        {
            Validate(sessionPricing);
        }

        public StandaloneSessionPricing(decimal sessionPrice)
            : base(sessionPrice)
        { }


        private void Validate(PricingData pricing)
        {
            if (_sessionPrice.Amount == null)
                throw new ValidationException("A sessionPrice is required.");
        }
    }
}
