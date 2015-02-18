using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class StandaloneSessionPricing : SingleSessionPricing
    {
        public new decimal SessionPrice { get { return _sessionPrice.Amount.Value; } }


        public StandaloneSessionPricing(PricingCommand sessionPricing, SingleSessionPricingData servicePricing)
            : base(sessionPricing, servicePricing)
        { }

        public StandaloneSessionPricing(decimal sessionPrice)
            : base(sessionPrice)
        { }

        protected override void ValidateAdditional(ValidationException errors, PricingCommand pricing)
        {
            if (!pricing.SessionPrice.HasValue)
                errors.Add("A sessionPrice is required.", "session.pricing.sessionPrice");

            if (pricing.CoursePrice.HasValue)
                errors.Add("The coursePrice field must not be specified for a single session.", "session.pricing.coursePrice");
        }
    }
}
