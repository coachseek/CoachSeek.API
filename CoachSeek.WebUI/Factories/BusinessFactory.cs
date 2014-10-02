using System.Collections.Generic;
using CoachSeek.WebUI.Contracts.Builders;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Persistence;
using CoachSeek.WebUI.Models.Requests;

namespace CoachSeek.WebUI.Factories
{
    public static class BusinessFactory
    {
        public static NewBusiness Create(BusinessRegistrationRequest registration, IBusinessDomainBuilder domainBuilder)
        {
            return new NewBusiness
            {
                Name = registration.BusinessName,
                Domain = domainBuilder.BuildDomain(registration.BusinessName),
                Admin = CreateBusinessAdmin(registration.Registrant)
            };
        }

        public static Business Create(DbBusiness dbBusiness)
        {
            if (dbBusiness == null)
                return null;

            return new Business
            {
                Identifier = new Identifier(dbBusiness.Id),
                Name = dbBusiness.Name,
                Domain = dbBusiness.Domain,
                Admin = CreateBusinessAdmin(dbBusiness.Admin),
                BusinessLocations = CreateBusinessLocations(dbBusiness.Locations)
            };
        }


        private static BusinessAdmin CreateBusinessAdmin(BusinessRegistrant registrant)
        {
            return new BusinessAdmin
            {
                FirstName = registrant.FirstName,
                LastName = registrant.LastName,
                Email = registrant.Email,
                Password = registrant.Password,
            };
        }

        private static BusinessAdmin CreateBusinessAdmin(DbBusinessAdmin dbAdmin)
        {
            return new BusinessAdmin
            {
                Id = new Identifier(dbAdmin.Id),
                FirstName = dbAdmin.FirstName,
                LastName = dbAdmin.LastName,
                Email = dbAdmin.Email,
                Password = dbAdmin.Password,
            };
        }

        private static BusinessLocations CreateBusinessLocations(IEnumerable<DbLocation> dbLocations)
        {
            var businessLocations = new BusinessLocations();

            foreach (var dbLocation in dbLocations)
            {
                var location = new Location
                {
                    Identifier = new Identifier(dbLocation.Id),
                    Name = dbLocation.Name
                };
                
                businessLocations.Add(location);
            }

            return businessLocations;
        }
    }
}