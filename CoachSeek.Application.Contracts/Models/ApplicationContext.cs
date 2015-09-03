using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class ApplicationContext : AdminApplicationContext
    {
        public BusinessContext BusinessContext { get; private set; }


        public ApplicationContext(UserContext userContext, 
                                  BusinessContext businessContext, 
                                  EmailContext emailContext,
                                  ILogRepository logRepository,
                                  bool isTesting) 
            : base(userContext, emailContext, logRepository, isTesting)
        {
            BusinessContext = businessContext;
        }
    }
}