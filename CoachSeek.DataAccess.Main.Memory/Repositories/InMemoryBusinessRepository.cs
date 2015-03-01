using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Conversion;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.Booking;
using CoachSeek.Domain.Repositories;
using Business = CoachSeek.Domain.Entities.Business;

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
        public static Dictionary<Guid, List<DbService>> Services { get; private set; }
        public static Dictionary<Guid, List<DbCustomer>> Customers { get; private set; }
        public static Dictionary<Guid, List<DbSingleSession>> Sessions { get; private set; }
        public static Dictionary<Guid, List<DbRepeatedSession>> Courses { get; private set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<DbBusiness>();

            Locations = new Dictionary<Guid, List<DbLocation>>();
            Coaches = new Dictionary<Guid, List<DbCoach>>();
            Services = new Dictionary<Guid, List<DbService>>();
            Customers = new Dictionary<Guid, List<DbCustomer>>();
            Sessions = new Dictionary<Guid, List<DbSingleSession>>();
            Courses = new Dictionary<Guid, List<DbRepeatedSession>>();
        }

        public void Clear()
        {
            Businesses.Clear();

            Locations.Clear();
            Coaches.Clear();
            Services.Clear();
            Customers.Clear();
            Sessions.Clear();
            Courses.Clear();
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


        public IList<ServiceData> GetAllServices(Guid businessId)
        {
            var dbServices = GetAllDbServices(businessId);

            return Mapper.Map<IList<DbService>, IList<ServiceData>>(dbServices);
        }

        public ServiceData GetService(Guid businessId, Guid serviceId)
        {
            var businessServices = GetAllServices(businessId);

            return businessServices.FirstOrDefault(x => x.Id == serviceId);
        }

        public ServiceData AddService(Guid businessId, Service service)
        {
            var dbService = Mapper.Map<Service, DbService>(service);

            var dbServices = GetAllDbServices(businessId);
            dbServices.Add(dbService);

            Services[businessId] = dbServices;

            return GetService(businessId, service.Id);
        }

        public ServiceData UpdateService(Guid businessId, Service service)
        {
            var dbService = Mapper.Map<Service, DbService>(service);

            var dbServices = GetAllDbServices(businessId);
            var index = dbServices.FindIndex(x => x.Id == service.Id);
            dbServices[index] = dbService;
            Services[businessId] = dbServices;

            return GetService(businessId, service.Id);
        }


        public IList<CustomerData> GetAllCustomers(Guid businessId)
        {
            var dbCustomers = GetAllDbCustomers(businessId);

            return Mapper.Map<IList<DbCustomer>, IList<CustomerData>>(dbCustomers);
        }

        public CustomerData GetCustomer(Guid businessId, Guid customerId)
        {
            var businessCustomers = GetAllCustomers(businessId);

            return businessCustomers.FirstOrDefault(x => x.Id == customerId);
        }

        public CustomerData AddCustomer(Guid businessId, Customer customer)
        {
            var dbCustomer = Mapper.Map<Customer, DbCustomer>(customer);

            var dbCustomers = GetAllDbCustomers(businessId);
            dbCustomers.Add(dbCustomer);

            Customers[businessId] = dbCustomers;

            return GetCustomer(businessId, customer.Id);
        }

        public CustomerData UpdateCustomer(Guid businessId, Customer customer)
        {
            var dbCustomer = Mapper.Map<Customer, DbCustomer>(customer);

            var dbCustomers = GetAllDbCustomers(businessId);
            var index = dbCustomers.FindIndex(x => x.Id == customer.Id);
            dbCustomers[index] = dbCustomer;
            Customers[businessId] = dbCustomers;

            return GetCustomer(businessId, customer.Id);
        }


        public IList<SingleSessionData> GetAllStandaloneSessions(Guid businessId)
        {
            var dbSessions = GetAllDbSessions(businessId);

            return Mapper.Map<IList<DbSingleSession>, IList<SingleSessionData>>(dbSessions);
        }

        public IList<SingleSessionData> GetAllSessions(Guid businessId)
        {
            var dbSessions = new List<DbSingleSession>(GetAllDbSessions(businessId));
            foreach (var dbCourse in GetAllDbCourses(businessId))
                dbSessions.AddRange(dbCourse.Sessions);

            return Mapper.Map<IList<DbSingleSession>, IList<SingleSessionData>>(dbSessions);
        }

        public SingleSessionData GetSession(Guid businessId, Guid sessionId)
        {
            // Standalone + Course Sessions
            var businessSessions = GetAllSessions(businessId);

            return businessSessions.FirstOrDefault(x => x.Id == sessionId);
        }

        public SingleSessionData AddSession(Guid businessId, StandaloneSession session)
        {
            var dbSession = Mapper.Map<StandaloneSession, DbSingleSession>(session);
            dbSession.Repetition = new DbRepetition { SessionCount = 1 };

            var dbSessions = GetAllDbSessions(businessId);
            dbSessions.Add(dbSession);

            Sessions[businessId] = dbSessions;

            return GetSession(businessId, session.Id);
        }

        public SingleSessionData UpdateSession(Guid businessId, StandaloneSession session)
        {
            var dbSession = Mapper.Map<StandaloneSession, DbSingleSession>(session);
            dbSession.Repetition = new DbRepetition { SessionCount = 1 };

            var dbSessions = GetAllDbSessions(businessId);
            var index = dbSessions.FindIndex(x => x.Id == session.Id);
            dbSessions[index] = dbSession;
            Sessions[businessId] = dbSessions;

            return GetSession(businessId, session.Id);
        }


        public IList<RepeatedSessionData> GetAllCourses(Guid businessId)
        {
            var dbCourses = GetAllDbCourses(businessId);

            return Mapper.Map<IList<DbRepeatedSession>, IList<RepeatedSessionData>>(dbCourses);
        }

        public RepeatedSessionData GetCourse(Guid businessId, Guid courseId)
        {
            var businessCourses = GetAllCourses(businessId);

            return businessCourses.FirstOrDefault(x => x.Id == courseId);
        }

        public RepeatedSessionData AddCourse(Guid businessId, RepeatedSession course)
        {
            var dbCourse = Mapper.Map<RepeatedSession, DbRepeatedSession>(course);

            var dbCourses = GetAllDbCourses(businessId);
            dbCourses.Add(dbCourse);

            Courses[businessId] = dbCourses;

            return GetCourse(businessId, course.Id);
        }


        public IList<BookingData> GetAllBookings(Guid businessId)
        {
            throw new NotImplementedException();
        }

        public BookingData AddBooking(Guid businessId, Booking booking)
        {
            throw new NotImplementedException();
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

        private List<DbService> GetAllDbServices(Guid businessId)
        {
            List<DbService> businessServices;
            return Services.TryGetValue(businessId, out businessServices)
                ? businessServices.OrderBy(x => x.Name).ToList()
                : new List<DbService>();
        }

        private List<DbCustomer> GetAllDbCustomers(Guid businessId)
        {
            List<DbCustomer> businessCustomers;
            return Customers.TryGetValue(businessId, out businessCustomers)
                ? businessCustomers
                : new List<DbCustomer>();
        }

        private List<DbSingleSession> GetAllDbSessions(Guid businessId)
        {
            List<DbSingleSession> businessSessions;
            return Sessions.TryGetValue(businessId, out businessSessions)
                ? businessSessions
                : new List<DbSingleSession>();
        }

        private List<DbRepeatedSession> GetAllDbCourses(Guid businessId)
        {
            List<DbRepeatedSession> businessCourses;
            return Courses.TryGetValue(businessId, out businessCourses)
                ? businessCourses
                : new List<DbRepeatedSession>();
        }
    }
}