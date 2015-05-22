namespace CoachSeek.Domain.Repositories
{
    public interface IUnsubscribedEmailAddressRepository
    {
        void Save(string emailAddress);
    }
}
