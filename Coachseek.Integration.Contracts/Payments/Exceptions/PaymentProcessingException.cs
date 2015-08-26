using System;

namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class PaymentProcessingException : Exception
    {
        public PaymentProcessingException()
        { }

        public PaymentProcessingException(string message) 
            : base(message)
        { }
    }
}
