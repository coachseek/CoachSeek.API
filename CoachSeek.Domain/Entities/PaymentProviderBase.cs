namespace CoachSeek.Domain.Entities
{
    public abstract class PaymentProviderBase
    {
        public abstract string Provider { get; }
        public string MerchantAccountIdentifier { get; protected set; }


        protected PaymentProviderBase(string merchantAccountIdentifier)
        {
            MerchantAccountIdentifier = merchantAccountIdentifier;
        }
    }
}
