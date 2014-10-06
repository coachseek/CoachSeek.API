using System;
using CoachSeek.WebUI.Models;

namespace CoachSeek.WebUI.Contracts.Persistence
{
    public interface IBusinessRepository
    {
        Business Save(NewBusiness newBusiness);
        Business Save(Business business);

        Business Get(Guid id);
        Business GetByDomain(string domain);
        Business GetByAdminEmail(string email);
    }
}
