using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class StandaloneSessionPricing : SingleSessionPricing
    {
        public new decimal SessionPrice { get { return _sessionPrice.Amount.GetValueOrDefault(); } }


        public StandaloneSessionPricing(PricingCommand sessionPricing)
            : base(sessionPricing)
        { }

        public StandaloneSessionPricing(decimal sessionPrice)
            : base(sessionPrice)
        { }

        protected override void ValidateAdditional(ValidationException errors, PricingCommand pricing)
        {
            if (!pricing.SessionPrice.HasValue)
                errors.Add(new StandaloneSessionMustHaveSessionPrice());
            if (pricing.CoursePrice.HasValue)
                errors.Add(new StandaloneSessionMustHaveNoCoursePrice());
        }
    }
}
