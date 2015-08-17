using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class PaymentProviderRequiredWhenOnlineBookingIsEnabled : SingleErrorException
    {
        public PaymentProviderRequiredWhenOnlineBookingIsEnabled()
            : base(ErrorCodes.PaymentProviderRequiredWhenOnlineBookingIsEnabled, 
                   "When Online Payment is enabled then an Online Payment Provider must be specified.")
        { }
    }
}
