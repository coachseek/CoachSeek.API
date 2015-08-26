namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class InvalidBooking : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Invalid booking."; }
        }
    }
}
