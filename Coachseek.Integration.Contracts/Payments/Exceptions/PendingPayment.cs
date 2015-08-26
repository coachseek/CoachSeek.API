namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class PendingPayment : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Pending payment."; }
        }
    }
}
