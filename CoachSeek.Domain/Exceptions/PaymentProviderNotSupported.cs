using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class PaymentProviderNotSupported : SingleErrorException
    {
        public PaymentProviderNotSupported(string paymentProvider)
            : base(ErrorCodes.PaymentProviderNotSupported, 
                   string.Format("Payment provider '{0}' is not supported.", paymentProvider),
                   paymentProvider)
        { }
    }
}
