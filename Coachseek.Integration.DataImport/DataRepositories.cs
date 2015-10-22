using CoachSeek.Domain.Repositories;

namespace Coachseek.Integration.DataImport
{
    public class DataRepositories
    {
        public IBusinessRepository BusinessRepository { get; private set; }
        public ILogRepository LogRepository { get; private set; }


        public DataRepositories(IBusinessRepository businessRepository,
                                ILogRepository logRepository)
        {
            BusinessRepository = businessRepository;
            LogRepository = logRepository;
        }
    }
}
