namespace CoachSeek.Domain.Entities
{
    public class NullPaymentProvider : PaymentProvider
    {
        public NullPaymentProvider() 
            : base(null)
        { }

        public override string Provider { get { return null; } }
    }
}
