using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class ServiceIsPricedButHasNoPrices : SingleErrorException
    {
        public ServiceIsPricedButHasNoPrices()
            : base(ErrorCodes.ServiceIsPricedButHasNoPrices,
                   "Service is priced but has neither session price nor course price.")
        { }
    }
}