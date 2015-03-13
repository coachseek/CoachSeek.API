using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Conversion;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.Booking;
using CoachSeek.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryBusinessRepository : IBusinessRepository
    {
        // Spy behaviour is included
        public bool WasAddBusinessCalled;
        public bool WasAddLocationCalled;
        public bool WasAddCoachCalled;

        public bool WasUpdateLocationCalled;
        public bool WasUpdateCoachCalled;

        public Guid BusinessIdPassedIn;
        public object DataPassedIn;

        public static List<DbBusiness> Businesses { get; private set; }
        public static Dictionary<Guid, List<DbLocation>> Locations { get; private set; }
        public static Dictionary<Guid, List<DbCoach>> Coaches { get; private set; }
        public static Dictionary<Guid, List<DbService>> Services { get; private set; }
        public static Dictionary<Guid, List<DbCustomer>> Customers { get; private set; }
        public static Dictionary<Guid, List<DbSingleSession>> Sessions { get; private set; }
        public static Dictionary<Guid, List<DbRepeatedSession>> Courses { get; private set; }
        public static Dictionary<Guid, List<DbBooking>> Bookings { get; private set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<DbBusiness>();

            Locations = new Dictionary<Guid, List<DbLocation>>();
            Coaches = new Dictionary<Guid, List<DbCoach>>();
            Services = new Dictionary<Guid, List<DbService>>();
            Customers = new Dictionary<Guid, List<DbCustomer>>();
            Sessions = new Dictionary<Guid, List<DbSingleSession>>();
            Courses = new Dictionary<Guid, List<DbRepeatedSession>>();
            Bookings = new Dictionary<Guid, List<DbBooking>>();
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
            Bookings.Clear();
        }


        public BusinessData GetBusiness(Guid businessId)
        {
            var business = Businesses.FirstOrDefault(x => x.Id == businessId);

            return Mapper.Map<DbBusiness, BusinessData>(business);
        }

        public BusinessData AddBusiness(Business business)
        {
            WasAddBusinessCalled = true;
            DataPassedIn = business;

            var dbBusiness = DbBusinessConverter.Convert(business);
            Businesses.Add(dbBusiness);

            return GetBusiness(business.Id);
        }

        public bool IsAvailableDomain(string domain)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Domain == domain);
            return dbBusiness == null;
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


        //// Only used for tests to add a business while bypassing the validation that occurs using Save.
        //public Business Add(Business business)
        //{
        //    var dbBusiness = DbBusinessConverter.Convert(business);

        //    Businesses.Add(dbBusiness);
        //    return business;
        //}


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
            WasAddLocationCalled = true;
            BusinessIdPassedIn = businessId;
            DataPassedIn = location;

            var dbLocation = Mapper.Map<Location, DbLocation>(location);

            var dbLocations = GetAllDbLocations(businessId);
            dbLocations.Add(dbLocation);

            Locations[businessId] = dbLocations;

            return GetLocation(businessId, location.Id);
        }

        public LocationData UpdateLocation(Guid businessId, Location location)
        {
            WasUpdateLocationCalled = true;
            BusinessIdPassedIn = businessId;
            DataPassedIn = location;

            var dbLocation = Mapper.Map<Location, DbLocation>(location);

            var dbLocations = GetAllDbLocations(businessId);
            var index = dbLocations.FindIndex(x => x.Id == location.Id);
            dbLocations[index] = dbLocation;
            Locations[businessId] = dbLocations;

            return GetLocation(businessId, location.Id);
        }


        public IList<CoachData> GetAllCoaches(Guid businessId)
        {
            var dbCoaches = GetAllDbCoaches(businessId).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();

            return Mapper.Map<IList<DbCoach>, IList<CoachData>>(dbCoaches);
        }

        public CoachData GetCoach(Guid businessId, Guid coachId)
        {
            var businessCoaches = GetAllCoaches(businessId);

            return businessCoaches.FirstOrDefault(x => x.Id == coachId);
        }

        public CoachData AddCoach(Guid businessId, Coach coach)
        {
            WasAddCoachCalled = true;
            BusinessIdPassedIn = businessId;
            DataPassedIn = coach;

            var dbCoach = Mapper.Map<Coach, DbCoach>(coach);

            var dbCoaches = GetAllDbCoaches(businessId);
            dbCoaches.Add(dbCoach);

            Coaches[businessId] = dbCoaches;

            return GetCoach(businessId, coach.Id);
        }

        public CoachData UpdateCoach(Guid businessId, Coach coach)
        {
            WasUpdateCoachCalled = true;
            BusinessIdPassedIn = businessId;
            DataPassedIn = coach;

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
            var dbCustomers = GetAllDbCustomers(businessId).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();

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

            var sessions =  Mapper.Map<IList<DbSingleSession>, IList<SingleSessionData>>(dbSessions);

            foreach (var session in sessions)
            {
                var location = GetLocation(businessId, session.Location.Id);
                session.Location.Name = location.Name;

                var coach = GetCoach(businessId, session.Coach.Id);
                session.Coach.Name = coach.Name;

                var service = GetService(businessId, session.Service.Id);
                session.Service.Name = service.Name;
            }

            return sessions;
        }

        public SingleSessionData GetSession(Guid businessId, Guid sessionId)
        {
            // Standalone + Course Sessions
            var businessSessions = GetAllSessions(businessId);

            var session = businessSessions.FirstOrDefault(x => x.Id == sessionId);
            if (session == null)
                return null;

            var location = GetLocation(businessId, session.Location.Id);
            session.Location.Name = location.Name;

            var coach = GetCoach(businessId, session.Coach.Id);
            session.Coach.Name = coach.Name;

            var service = GetService(businessId, session.Service.Id);
            session.Service.Name = service.Name;

            return session;
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

        public SingleSessionData UpdateSession(Guid businessId, SingleSession session)
        {
            var dbSession = Mapper.Map<SingleSession, DbSingleSession>(session);
            dbSession.Repetition = new DbRepetition { SessionCount = 1 };

            var dbSessions = GetAllDbSessions(businessId);
            var index = dbSessions.FindIndex(x => x.Id == session.Id);
            if (index == -1)
            {
                // Update session in course
                var dbCourses = GetAllDbCourses(businessId);
                foreach (var dbCourse in dbCourses)
                {
                    index = dbCourse.Sessions.ToList().FindIndex(x => x.Id == session.Id);
                    if (index == -1)
                        continue;
                    var existingSession = dbCourse.Sessions[index];
                    dbSession.ParentId = existingSession.ParentId;

                    dbCourse.Sessions[index] = dbSession;
                }

                return GetSession(businessId, session.Id);
            }

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

            var course = businessCourses.FirstOrDefault(x => x.Id == courseId);
            if (course == null)
                return null;

            var location = GetLocation(businessId, course.Location.Id);
            course.Location.Name = location.Name;

            var coach = GetCoach(businessId, course.Coach.Id);
            course.Coach.Name = coach.Name;

            var service = GetService(businessId, course.Service.Id);
            course.Service.Name = service.Name;

            return course;
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
            var dbBookings = GetAllDbBookings(businessId);

            return Mapper.Map<IList<DbBooking>, IList<BookingData>>(dbBookings);
        }

        public BookingData GetBooking(Guid businessId, Guid bookingId)
        {
            var businessBookings = GetAllBookings(businessId);

            var booking = businessBookings.FirstOrDefault(x => x.Id == bookingId);
            if (booking == null)
                return null;

            var session = GetSession(businessId, booking.Session.Id);
            booking.Session.Name = string.Format("{0} at {1} with {2} on {3} at {4}",
                                                 session.Service.Name,
                                                 session.Location.Name,
                                                 session.Coach.Name,
                                                 session.Timing.StartDate,
                                                 session.Timing.StartTime);

            var customer = GetCustomer(businessId, booking.Customer.Id);
            booking.Customer.Name = string.Format("{0} {1}", customer.FirstName, customer.LastName);

            return booking;
        }

        public BookingData AddBooking(Guid businessId, Booking booking)
        {
            var dbBooking = Mapper.Map<Booking, DbBooking>(booking);

            var dbBookings = GetAllDbBookings(businessId);
            dbBookings.Add(dbBooking);

            Bookings[businessId] = dbBookings;

            return GetBooking(businessId, booking.Id);
        }


        public void DeleteBooking(Guid businessId, Guid bookingId)
        {
            var dbBookings = GetAllDbBookings(businessId);

            var dbBooking = dbBookings.Find(x => x.Id == bookingId);

            dbBookings.Remove(dbBooking);

            Bookings[businessId] = dbBookings;
        }


        public IList<CustomerBookingData> GetCustomerBookingsBySessionId(Guid businessId, Guid sessionId)
        {
            var bookings = GetAllBookings(businessId);
            var sessionBookings = bookings.Where(x => x.Session.Id == sessionId);

            return sessionBookings.Select(sessionBooking => new CustomerBookingData
            {
                BookingId = sessionBooking.Id, 
                Customer = GetCustomer(businessId, sessionBooking.Customer.Id)
            }).ToList();
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

        private List<DbBooking> GetAllDbBookings(Guid businessId)
        {
            List<DbBooking> businessBookings;
            return Bookings.TryGetValue(businessId, out businessBookings)
                ? businessBookings
                : new List<DbBooking>();
        }
    }
}