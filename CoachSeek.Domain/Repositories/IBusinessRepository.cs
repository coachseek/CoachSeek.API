using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using System;

namespace CoachSeek.Domain.Repositories
{
    public interface IBusinessRepository
    {
        Business2Data GetBusiness(Guid businessId);
        Business2Data AddBusiness(Business2 business);


        Business Save(Business business);

        Business Get(Guid id);

        bool IsAvailableDomain(string domain);


        IList<LocationData> GetAllLocations(Guid businessId);
        LocationData GetLocation(Guid businessId, Guid locationId);
        LocationData AddLocation(Guid businessId, Location location);
        LocationData UpdateLocation(Guid businessId, Location location);

        IList<CoachData> GetAllCoaches(Guid businessId);
        CoachData GetCoach(Guid businessId, Guid coachId);
        CoachData AddCoach(Guid businessId, Coach coach);
        CoachData UpdateCoach(Guid businessId, Coach coach);
    }
}
