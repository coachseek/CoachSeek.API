using System;

namespace Coachseek.Integration.Contracts.Exceptions
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
