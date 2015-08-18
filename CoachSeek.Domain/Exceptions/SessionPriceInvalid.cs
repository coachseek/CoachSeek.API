using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class SessionPriceInvalid : PriceInvalid
    {
        public SessionPriceInvalid(decimal sessionPrice)
            : base(ErrorCodes.SessionPriceInvalid,
                   string.Format("A SessionPrice of {0} is not valid.", sessionPrice),
                   sessionPrice)
        { }

        public SessionPriceInvalid(PriceInvalid priceInvalid)
            : base(ErrorCodes.SessionPriceInvalid,
                   string.Format("A SessionPrice of {0} is not valid.", priceInvalid.Price),
                   priceInvalid.Price)
        { }
    }
}
