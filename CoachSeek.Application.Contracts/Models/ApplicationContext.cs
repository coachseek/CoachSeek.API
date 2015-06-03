namespace CoachSeek.Application.Contracts.Models
{
    public class ApplicationContext
    {
        public BusinessContext BusinessContext { get; private set; }
        public EmailContext EmailContext { get; private set; }
        public PaymentContext PaymentContext { get; private set; }
        public bool IsTesting { get; private set; }


        public ApplicationContext(BusinessContext businessContext, EmailContext emailContext, PaymentContext paymentContext, bool isTesting)
        {
            BusinessContext = businessContext;
            EmailContext = emailContext;
            PaymentContext = paymentContext;
            IsTesting = isTesting;
        }
    }
}