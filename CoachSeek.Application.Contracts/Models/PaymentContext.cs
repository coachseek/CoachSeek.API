namespace CoachSeek.Application.Contracts.Models
{
    public class PaymentContext
    {
        public bool IsPaymentEnabled { get; private set; }


        public PaymentContext(bool isPaymentEnabled)
        {
            IsPaymentEnabled = isPaymentEnabled;
        }
    }
}
