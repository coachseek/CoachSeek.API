
using CoachSeek.WebUI.Models;

namespace CoachSeek.WebUI.Contracts.Persistence
{
    public interface IBusinessAdminRepository
    {
        BusinessAdmin Add(BusinessAdmin admin);

        BusinessAdmin GetByEmail(string email);
    }
}
