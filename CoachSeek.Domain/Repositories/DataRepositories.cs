namespace CoachSeek.Domain.Repositories
{
    public class DataRepositories
    {
        public IBusinessRepository BusinessRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { get; private set; }


        public DataRepositories(IBusinessRepository businessRepository, 
                                IUserRepository userRepository,
                                IUnsubscribedEmailAddressRepository unsubscribedEmailAddressRepository)
        {
            BusinessRepository = businessRepository;
            UserRepository = userRepository;
            UnsubscribedEmailAddressRepository = unsubscribedEmailAddressRepository;
        }
    }
}
