using CoachSeek.Domain.Entities;
using System;

namespace CoachSeek.Domain.Repositories
{
    public interface IBusinessRepository
    {
        Business Save(NewBusiness newBusiness);
        Business Save(Business business);

        Business Get(Guid id);
        Business GetByDomain(string domain);
        //Business GetByAdminEmail(string email);
    }
}
