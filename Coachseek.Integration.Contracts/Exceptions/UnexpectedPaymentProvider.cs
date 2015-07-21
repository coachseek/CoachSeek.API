namespace Coachseek.Integration.Contracts.Exceptions
{
    public class UnexpectedPaymentProvider : PaymentProcessingException
    {
        private string PaymentProvider { get; set; }

        public UnexpectedPaymentProvider(string paymentProvider)
        {
            PaymentProvider = paymentProvider;
        }


        public override string Message
        {
            get { return string.Format("Unexpected payment provider: '{0}'", PaymentProvider); }
        }
    }
}
