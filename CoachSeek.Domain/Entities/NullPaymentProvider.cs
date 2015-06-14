namespace CoachSeek.Domain.Entities
{
    public class NullPaymentProvider : PaymentProviderBase
    {
        public NullPaymentProvider() 
            : base(null)
        { }

        public override string Provider { get { return null; } }
    }
}
