using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Conversion;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryBusinessRepository : IBusinessRepository
    {
        // Spy behaviour is included
        public bool WasSaveNewBusinessCalled;
        public bool WasSaveBusinessCalled;

        public static List<DbBusiness> Businesses { get; private set; }
        public static Dictionary<Guid, List<DbLocation>> Locations { get; private set; }
        public static Dictionary<Guid, List<DbCoach>> Coaches { get; private set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<DbBusiness>();

            Locations = new Dictionary<Guid, List<DbLocation>>();
            Coaches = new Dictionary<Guid, List<DbCoach>>();
        }

        public void Clear()
        {
            Businesses.Clear();

            Locations.Clear();
            Coaches.Clear();
        }


        public Business2Data GetBusiness(Guid businessId)
        {
            var business = Businesses.FirstOrDefault(x => x.Id == businessId);

            return Mapper.Map<DbBusiness, Business2Data>(business);
        }

        public Business2Data AddBusiness(Business2 business)
        {
            WasSaveNewBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(business);
            Businesses.Add(dbBusiness);

            return GetBusiness(business.Id);
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

        public bool IsAvailableDomain(string domain)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Domain == domain);
            return dbBusiness == null;
        }


        private Business CreateBusiness(DbBusiness dbBusiness)
        {
            if (dbBusiness == null)
                return null;

            var locations = Mapper.Map<IEnumerable<DbLocation>, IEnumerable<LocationData>>(dbBusiness.Locations);
            var coaches = Mapper.Map<IEnumerable<DbCoach>, IEnumerable<CoachData>>(dbBusiness.Coaches);
            var services = Mapper.Map<IEnumerable<DbService>, IEnumerable<ServiceData>>(dbBusiness.Services);
            var sessions = Mapper.Map<IEnumerable<DbSingleSession>, IEnumerable<SingleSessionData>>(dbBusiness.Sessions);
            var courses = Mapper.Map<IEnumerable<DbRepeatedSession>, IEnumerable<RepeatedSessionData>>(dbBusiness.Courses);
            var customers = Mapper.Map<IEnumerable<DbCustomer>, IEnumerable<CustomerData>>(dbBusiness.Customers);

            return new Business(dbBusiness.Id,
                dbBusiness.Name,
                dbBusiness.Domain,
                locations,
                coaches,
                services,
                sessions,
                courses,
                customers);
        }


        private static BusinessAdminData CreateBusinessAdmin(DbBusinessAdmin dbAdmin)
        {
            return new BusinessAdmin(dbAdmin.Id, dbAdmin.Email,
                                     dbAdmin.FirstName, dbAdmin.LastName,
                                     dbAdmin.Username, dbAdmin.PasswordHash).ToData();
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


        public IList<LocationData> GetAllLocations(Guid businessId)
        {
            var dbLocations = GetAllDbLocations(businessId);

            return Mapper.Map<IList<DbLocation>, IList<LocationData>>(dbLocations);
        }

        public LocationData GetLocation(Guid businessId, Guid locationId)
        {
            var businessLocations = GetAllLocations(businessId);

            return businessLocations.FirstOrDefault(x => x.Id == locationId);
        }

        public LocationData AddLocation(Guid businessId, Location location)
        {
            var dbLocation = Mapper.Map<Location, DbLocation>(location);

            var dbLocations = GetAllDbLocations(businessId);
            dbLocations.Add(dbLocation);

            Locations[businessId] = dbLocations;

            return GetLocation(businessId, location.Id);
        }

        public LocationData UpdateLocation(Guid businessId, Location location)
        {
            var dbLocation = Mapper.Map<Location, DbLocation>(location);

            var dbLocations = GetAllDbLocations(businessId);
            var index = dbLocations.FindIndex(x => x.Id == location.Id);
            dbLocations[index] = dbLocation;
            Locations[businessId] = dbLocations;

            return GetLocation(businessId, location.Id);
        }


        public IList<CoachData> GetAllCoaches(Guid businessId)
        {
            var dbCoaches = GetAllDbCoaches(businessId);

            return Mapper.Map<IList<DbCoach>, IList<CoachData>>(dbCoaches);
        }

        public CoachData GetCoach(Guid businessId, Guid coachId)
        {
            var businessCoaches = GetAllCoaches(businessId);

            return businessCoaches.FirstOrDefault(x => x.Id == coachId);
        }

        public CoachData AddCoach(Guid businessId, Coach coach)
        {
            var dbCoach = Mapper.Map<Coach, DbCoach>(coach);

            var dbCoaches = GetAllDbCoaches(businessId);
            dbCoaches.Add(dbCoach);

            Coaches[businessId] = dbCoaches;

            return GetCoach(businessId, coach.Id);
        }

        public CoachData UpdateCoach(Guid businessId, Coach coach)
        {
            var dbCoach = Mapper.Map<Coach, DbCoach>(coach);

            var dbCoaches = GetAllDbCoaches(businessId);
            var index = dbCoaches.FindIndex(x => x.Id == coach.Id);
            dbCoaches[index] = dbCoach;
            Coaches[businessId] = dbCoaches;

            return GetCoach(businessId, coach.Id);
        }


        private List<DbLocation> GetAllDbLocations(Guid businessId)
        {
            List<DbLocation> businessLocations;
            return Locations.TryGetValue(businessId, out businessLocations)
                ? businessLocations
                : new List<DbLocation>();
        }

        private List<DbCoach> GetAllDbCoaches(Guid businessId)
        {
            List<DbCoach> businessCoaches;
            return Coaches.TryGetValue(businessId, out businessCoaches)
                ? businessCoaches
                : new List<DbCoach>();
        }
    }
}