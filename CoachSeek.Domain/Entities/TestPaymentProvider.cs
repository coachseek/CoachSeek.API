namespace CoachSeek.Domain.Entities
{
    public class TestPaymentProvider : PaymentProviderBase
    {
        public TestPaymentProvider(bool isOnlinePaymentEnabled, string merchantAccountIdentifier)
            : base(isOnlinePaymentEnabled, merchantAccountIdentifier)
        { }

        public override string ProviderName { get { return "Test"; } }
    }
}
