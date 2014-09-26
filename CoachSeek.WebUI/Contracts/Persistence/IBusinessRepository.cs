using CoachSeek.WebUI.Models;

namespace CoachSeek.WebUI.Contracts.Persistence
{
    public interface IBusinessRepository
    {
        Business Add(Business business);

        Business GetByDomain(string domain);
    }
}
