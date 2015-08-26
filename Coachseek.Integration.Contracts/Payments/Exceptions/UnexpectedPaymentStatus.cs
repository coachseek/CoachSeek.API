namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class UnexpectedPaymentStatus : PaymentProcessingException
    {
        private string PaymentProvider { get; set; }
        private string PaymentStatus { get; set; }

        public UnexpectedPaymentStatus(string paymentProvider, string paymentStatus)
        {
            PaymentProvider = paymentProvider;
            PaymentStatus = paymentStatus;
        }


        public override string Message
        {
            get { return string.Format("Unexpected {0} payment status: '{1}'", PaymentProvider, PaymentStatus); }
        }
    }
}
