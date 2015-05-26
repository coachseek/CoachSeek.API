namespace CoachSeek.Application.Contracts.Models
{
    public class ApplicationContext
    {
        public BusinessContext Business { get; private set; }
        public EmailContext Email { get; private set; }
        public bool IsTesting { get; private set; }


        public ApplicationContext(BusinessContext businessContext, EmailContext emailContext, bool isTesting)
        {
            Business = businessContext;
            Email = emailContext;
            IsTesting = isTesting;
        }
    }
}