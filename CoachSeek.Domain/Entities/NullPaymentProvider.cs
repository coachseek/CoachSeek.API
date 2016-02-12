namespace CoachSeek.Domain.Entities
{
    public class NullPaymentProvider : PaymentProviderBase
    {
        public NullPaymentProvider() 
            : base(true, null)
        { }

        public override string ProviderName { get { return null; } }
    }
}
