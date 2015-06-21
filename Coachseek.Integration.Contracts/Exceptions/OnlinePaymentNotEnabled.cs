namespace Coachseek.Integration.Contracts.Exceptions
{
    public class OnlinePaymentNotEnabled : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Online payment is not enabled."; }
        }
    }
}
