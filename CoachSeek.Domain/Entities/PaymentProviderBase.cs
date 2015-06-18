namespace CoachSeek.Domain.Entities
{
    public abstract class PaymentProviderBase
    {
        public abstract string ProviderName { get; }
        public string MerchantAccountIdentifier { get; protected set; }


        protected PaymentProviderBase(string merchantAccountIdentifier)
        {
            MerchantAccountIdentifier = merchantAccountIdentifier;
        }
    }
}
