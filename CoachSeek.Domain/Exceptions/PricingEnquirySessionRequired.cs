using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class PricingEnquirySessionRequired : SingleErrorException
    {
        public PricingEnquirySessionRequired()
            : base(ErrorCodes.PricingSessionRequired, "A pricing enquiry must have at least one session.")
        { }
    }
}
