using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public abstract class PaymentProvider
    {
        public abstract string Provider { get; }
        public string MerchantAccountIdentifier { get; protected set; }


        protected PaymentProvider(string merchantAccountIdentifier)
        {
            MerchantAccountIdentifier = merchantAccountIdentifier;
        }
    }
}
