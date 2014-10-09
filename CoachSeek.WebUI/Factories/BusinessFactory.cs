using CoachSeek.Domain;
using CoachSeek.Domain.Data;
using CoachSeek.WebUI.Contracts.Builders;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Persistence;
using CoachSeek.WebUI.Models.Requests;
using System.Collections.Generic;

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

            return new Business(CreateLocations(dbBusiness.Locations), CreateCoaches(dbBusiness.Coaches))
            {
                Id = dbBusiness.Id,
                Name = dbBusiness.Name,
                Domain = dbBusiness.Domain,
                Admin = CreateBusinessAdmin(dbBusiness.Admin),
            };
        }


        private static BusinessAdmin CreateBusinessAdmin(BusinessRegistrant registrant)
        {
            return new BusinessAdmin(registrant.FirstName, registrant.LastName, registrant.Email, registrant.Password);
        }

        private static BusinessAdmin CreateBusinessAdmin(DbBusinessAdmin dbAdmin)
        {
            return new BusinessAdmin(dbAdmin.Id, dbAdmin.Email, 
                                     dbAdmin.FirstName, dbAdmin.LastName, 
                                     dbAdmin.Username, dbAdmin.PasswordHash, dbAdmin.PasswordSalt);
        }

        private static IEnumerable<Location> CreateLocations(IEnumerable<DbLocation> dbLocations)
        {
            var locations = new List<Location>();

            foreach (var dbLocation in dbLocations)
            {
                var location = new Location
                {
                    Id = dbLocation.Id,
                    Name = dbLocation.Name
                };

                locations.Add(location);
            }

            return locations;
        }

        private static IEnumerable<CoachData> CreateCoaches(IEnumerable<DbCoach> dbCoaches)
        {
            var coaches = new List<CoachData>();

            foreach (var dbCoach in dbCoaches)
            {
                var coach = new CoachData
                {
                    Id = dbCoach.Id,
                    FirstName = dbCoach.FirstName,
                    LastName = dbCoach.LastName,
                    Email = dbCoach.Email,
                    Phone = dbCoach.Phone
                };

                coaches.Add(coach);
            }

            return coaches;
        }
    }
}