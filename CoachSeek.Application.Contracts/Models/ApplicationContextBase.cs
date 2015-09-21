using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public abstract class ApplicationContextBase
    {
        public UserContext UserContext { get; protected set; }
        public EmailContext EmailContext { get; protected set; }
        public ILogRepository LogRepository { get; protected set; }
        public bool IsTesting { get; protected set; }


        protected ApplicationContextBase(UserContext userContext, 
                                         EmailContext emailContext, 
                                         ILogRepository logRepository,
                                         bool isTesting)
        {
            UserContext = userContext;
            EmailContext = emailContext;
            LogRepository = logRepository;
            IsTesting = isTesting;
        }
    }
}