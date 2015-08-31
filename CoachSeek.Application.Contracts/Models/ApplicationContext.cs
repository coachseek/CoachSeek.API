namespace CoachSeek.Application.Contracts.Models
{
    public class ApplicationContext : AdminApplicationContext
    {
        public BusinessContext BusinessContext { get; private set; }


        public ApplicationContext(UserContext userContext, 
                                  BusinessContext businessContext, 
                                  EmailContext emailContext, 
                                  bool isTesting) 
            : base(userContext, emailContext, isTesting)
        {
            BusinessContext = businessContext;
        }
    }
}