using CoachSeek.Domain.Exceptions;

namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class PaymentConversionException : PaymentProcessingException
    {
        private ValidationException ValidationException { get; set; }

        public PaymentConversionException(ValidationException ex)
        {
            ValidationException = ex;
        }

        public override string Message
        {
            get
            {
                var errorString = string.Empty;
                foreach (var error in ValidationException.Errors)
                    errorString = string.Format("{0} {1}", errorString, error.Message);
                return errorString.Trim();
            }
        }
    }
}
