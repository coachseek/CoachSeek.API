namespace CoachSeek.Domain.Repositories
{
    public interface IReservedDomainRepository
    {
        bool Contains(string domain);
    }
}
