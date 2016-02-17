using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class DiscountCodeDuplicate : SingleErrorException
    {
        public DiscountCodeDuplicate(DiscountCode discountCode)
            : base(ErrorCodes.DiscountCodeDuplicate,
                   string.Format("Discount Code '{0}' already exists.", discountCode.Code),
                   discountCode.Code)
        { }
    }
}