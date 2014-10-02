using System.Collections.Generic;
using System.Linq;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Persistence;

namespace CoachSeek.WebUI.Conversion
{
    public static class DbBusinessConverter
    {
        public static DbBusiness Convert(Business business)
        {
            return new DbBusiness
            {
                Id = business.Identifier.Id,
                Name = business.Name,
                Domain = business.Domain,
                Admin = Convert(business.Identifier, business.Admin),
                Locations = Convert(business.Identifier, business.BusinessLocations)
            };
        }

        private static DbBusinessAdmin Convert(Identifier businessId, BusinessAdmin admin)
        {
            return new DbBusinessAdmin
            {
                Id = admin.Id.Id,
                BusinessId = businessId.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Password = admin.Password
            };
        }

        private static List<DbLocation> Convert(Identifier businessId, BusinessLocations locations)
        {
            return locations.Locations.Select(location => new DbLocation()
            {
                Id = location.Identifier.Id, 
                BusinessId = businessId.Id, 
                Name = location.Name
            }).ToList();
        }
    }
}