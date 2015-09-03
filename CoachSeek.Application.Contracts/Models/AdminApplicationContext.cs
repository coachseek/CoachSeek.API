using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class AdminApplicationContext
    {
        public UserContext UserContext { get; private set; }
        public EmailContext EmailContext { get; private set; }
        public ILogRepository LogRepository { get; private set; }
        public bool IsTesting { get; private set; }


        public AdminApplicationContext(UserContext userContext, 
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