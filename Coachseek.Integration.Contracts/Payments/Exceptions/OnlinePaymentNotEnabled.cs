namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class OnlinePaymentNotEnabled : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Online payment is not enabled."; }
        }
    }
}
