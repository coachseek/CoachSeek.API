namespace CoachSeek.Domain.Entities.Payment
{
    public class NullPaymentProvider : PaymentProvider
    {
        public NullPaymentProvider() 
            : base(null)
        { }

        public override string Provider { get { return null; } }
    }
}
