namespace Coachseek.Integration.Contracts.Exceptions
{
    public class IsTestingStatusMismatch : PaymentProcessingException
    {
        public override string Message
        {
            get { return "IsTesting Status mismatch."; }
        }
    }
}
