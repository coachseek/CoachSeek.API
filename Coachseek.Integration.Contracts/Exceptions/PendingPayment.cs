namespace Coachseek.Integration.Contracts.Exceptions
{
    public class PendingPayment : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Pending payment."; }
        }
    }
}
