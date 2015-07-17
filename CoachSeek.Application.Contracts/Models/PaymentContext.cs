namespace CoachSeek.Application.Contracts.Models
{
    public class PaymentContext
    {
        private bool IsPaymentEnabled { get; set; }


        public PaymentContext(bool isPaymentEnabled)
        {
            IsPaymentEnabled = isPaymentEnabled;
        }
    }
}
