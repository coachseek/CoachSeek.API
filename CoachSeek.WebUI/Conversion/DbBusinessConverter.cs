using System;
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
                Id = business.Id,
                Name = business.Name,
                Domain = business.Domain,
                Admin = Convert(business.Id, business.Admin),
                Locations = Convert(business.Id, business.Locations)
            };
        }

        private static DbBusinessAdmin Convert(Guid businessId, BusinessAdmin admin)
        {
            return new DbBusinessAdmin
            {
                Id = admin.Id,
                BusinessId = businessId,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Password = admin.Password
            };
        }

        private static List<DbLocation> Convert(Guid businessId, IEnumerable<Location> locations)
        {
            return locations.Select(location => new DbLocation()
            {
                Id = location.Id, 
                BusinessId = businessId, 
                Name = location.Name
            }).ToList();
        }
    }
}