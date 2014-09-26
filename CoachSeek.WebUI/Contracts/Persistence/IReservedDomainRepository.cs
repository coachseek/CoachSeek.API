namespace CoachSeek.WebUI.Contracts.Persistence
{
    public interface IReservedDomainRepository
    {
        bool Contains(string domain);
    }
}
