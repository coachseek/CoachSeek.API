using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class DiscountCodeInvalid : SingleErrorException
    {
        public DiscountCodeInvalid(Guid discountCodeId)
            : base(ErrorCodes.DiscountCodeInvalid, 
                   "This discount code does not exist.",
                   discountCodeId.ToString())
        { }
    }
}