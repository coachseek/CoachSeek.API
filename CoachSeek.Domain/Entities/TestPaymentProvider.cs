namespace CoachSeek.Domain.Entities
{
    public class TestPaymentProvider : PaymentProviderBase
    {
        public TestPaymentProvider(string merchantAccountIdentifier)
            : base(merchantAccountIdentifier)
        { }

        public override string ProviderName { get { return "Test"; } }
    }
}
