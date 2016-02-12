namespace CoachSeek.Domain.Entities
{
    public abstract class PaymentProviderBase
    {
        public abstract string ProviderName { get; }
        public bool IsOnlinePaymentEnabled { get; protected set; }
        public string MerchantAccountIdentifier { get; protected set; }


        protected PaymentProviderBase(bool isOnlinePaymentEnabled, string merchantAccountIdentifier)
        {
            IsOnlinePaymentEnabled = isOnlinePaymentEnabled;
            MerchantAccountIdentifier = merchantAccountIdentifier;
        }
    }
}
