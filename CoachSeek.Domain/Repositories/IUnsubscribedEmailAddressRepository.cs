namespace CoachSeek.Domain.Repositories
{
    public interface IUnsubscribedEmailAddressRepository
    {
        bool Get(string emailAddress);
        void Save(string emailAddress);
    }
}
