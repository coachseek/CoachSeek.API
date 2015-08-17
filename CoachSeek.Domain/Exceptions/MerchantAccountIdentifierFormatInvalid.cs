using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class MerchantAccountIdentifierFormatInvalid : SingleErrorException
    {
        public MerchantAccountIdentifierFormatInvalid()
            : base(ErrorCodes.MerchantAccountIdentifierFormatInvalid,
                   "The MerchantAccountIdentifier field is not in a valid format.")
        { }
    }
}
