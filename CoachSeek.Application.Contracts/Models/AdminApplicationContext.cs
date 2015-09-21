using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class AdminApplicationContext : ApplicationContextBase
    {
        public IBusinessRepository BusinessRepository { get; private set; }


        public AdminApplicationContext(UserContext userContext,
                                       EmailContext emailContext,
                                       IBusinessRepository businessRepository, 
                                       ILogRepository logRepository,
                                       bool isTesting)
            : base(userContext, emailContext, logRepository, isTesting)
        {
            BusinessRepository = businessRepository;
        }
    }
}