using CoachSeek.Domain.Repositories;

namespace Coachseek.Integration.DataImport
{
    public class DataRepositories
    {
        public IBusinessRepository BusinessRepository { get; private set; }
        public ILogRepository LogRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { get; private set; }


        public DataRepositories(IBusinessRepository businessRepository,
                                ILogRepository logRepository,
                                IUserRepository userRepository,
                                IUnsubscribedEmailAddressRepository unsubscribedEmailAddressRepository)
        {
            BusinessRepository = businessRepository;
            LogRepository = logRepository;
            UserRepository = userRepository;
            UnsubscribedEmailAddressRepository = unsubscribedEmailAddressRepository;
        }
    }
}
