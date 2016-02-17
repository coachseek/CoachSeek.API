using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DiscountCodeIsActiveRequired : SingleErrorException
    {
        public DiscountCodeIsActiveRequired()
            : base(ErrorCodes.DiscountCodeIsActiveRequired,
                   "The discount code isActive field is required.")
        { }
    }
}