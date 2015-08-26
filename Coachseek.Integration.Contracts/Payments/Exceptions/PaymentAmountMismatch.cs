namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class PaymentAmountMismatch : PaymentProcessingException
    {
        private decimal PaymentAmount { get; set; }
        private decimal DueAmount { get; set; }

        public PaymentAmountMismatch(decimal paymentAmount, decimal dueAmount)
        {
            PaymentAmount = paymentAmount;
            DueAmount = dueAmount;
        }


        public override string Message
        {
            get { return string.Format("The payment amount ({0:C2}) does not match the due amount ({1:C2}).", PaymentAmount, DueAmount); }
        }
    }
}
