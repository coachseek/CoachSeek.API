using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DiscountCodeNotActive : SingleErrorException
    {
        public DiscountCodeNotActive(string discountCode)
            : base(ErrorCodes.DiscountCodeNotActive,
                   "This discount code is not active.",
                   discountCode)
        { }
    }
}