using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Conversion;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.DataAccess.Repositories
{
    public class InMemoryBusinessRepository : IBusinessRepository
    {
        // Spy behaviour is included
        public bool WasSaveNewBusinessCalled;
        public bool WasSaveBusinessCalled; 

        public static List<DbBusiness> Businesses { get; private set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<DbBusiness>();
        }

        public void Clear()
        {
            Businesses.Clear();
        }

        public Business Save(NewBusiness newBusiness)
        {
            WasSaveNewBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(newBusiness);

            Businesses.Add(dbBusiness);
            return newBusiness;
        }

        public Business Save(Business business)
        {
            WasSaveBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(business);
            var existingBusiness = Businesses.Single(x => x.Id == dbBusiness.Id);
            var existingIndex = Businesses.IndexOf(existingBusiness);
            Businesses[existingIndex] = dbBusiness;
            var updateBusiness = Businesses.Single(x => x.Id == dbBusiness.Id);
            return CreateBusiness(updateBusiness);
        }

        public Business Get(Guid id)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Id == id);
            return CreateBusiness(dbBusiness);
        }

        public Business GetByDomain(string domain)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Domain == domain);
            return CreateBusiness(dbBusiness);
        }

        public Business GetByAdminEmail(string adminEmail)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Admin.Email == adminEmail);
            return CreateBusiness(dbBusiness);
        }




        private Business CreateBusiness(DbBusiness dbBusiness)
        {
            if (dbBusiness == null)
                return null;

            return new Business(dbBusiness.Id,
                dbBusiness.Name,
                dbBusiness.Domain,
                Mapper.Map<DbBusinessAdmin, BusinessAdminData>(dbBusiness.Admin),
                Mapper.Map<IEnumerable<DbLocation>, IEnumerable<LocationData>>(dbBusiness.Locations),
                Mapper.Map<IEnumerable<DbCoach>, IEnumerable<CoachData>>(dbBusiness.Coaches));
        }


        private static BusinessAdminData CreateBusinessAdmin(DbBusinessAdmin dbAdmin)
        {
            return new BusinessAdmin(dbAdmin.Id, dbAdmin.Email,
                                     dbAdmin.FirstName, dbAdmin.LastName,
                                     dbAdmin.Username, dbAdmin.PasswordHash, dbAdmin.PasswordSalt).ToData();
        }

        private static IEnumerable<LocationData> CreateLocations(IEnumerable<DbLocation> dbLocations)
        {
            var locations = new List<LocationData>();

            foreach (var dbLocation in dbLocations)
            {
                var location = new LocationData
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


        // Only used for tests to add a business while bypassing the validation that occurs using Save.
        public Business Add(Business business)
        {
            var dbBusiness = DbBusinessConverter.Convert(business);

            Businesses.Add(dbBusiness);
            return business;
        }
    }
}