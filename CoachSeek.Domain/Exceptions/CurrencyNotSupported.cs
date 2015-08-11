using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CurrencyNotSupported : SingleErrorException
    {
        public CurrencyNotSupported(string currencyCode)
            : base(string.Format("Currency '{0}' is not supported.", currencyCode),
                   ErrorCodes.CurrencyNotSupported,
                   currencyCode)
        { }
    }
}
