using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Conversion;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.EmailTemplating;
using CoachSeek.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryBusinessRepository : IBusinessRepository
    {
        // Spy behaviour is included
        public bool WasGetBusinessByIdCalled;
        public bool WasAddBusinessCalled;
        public bool WasUpdateBusinessCalled;
        public bool WasAddLocationCalled;
        public bool WasUpdateLocationCalled;
        public bool WasAddCoachCalled;
        public bool WasUpdateCoachCalled;
        public bool WasGetSessionCalled;
        public bool WasGetAllCustomerBookingsCalled;
        public bool WasSetBookingPaymentStatusCalled;
        public bool WasSetBookingAttendanceCalled;
        public bool WasDeleteBookingCalled;

        public Guid BusinessIdPassedIn;
        public object DataPassedIn;

        public static List<DbBusiness> Businesses { get; private set; }
        public static Dictionary<Guid, List<DbLocation>> Locations { get; private set; }
        public static Dictionary<Guid, List<DbCoach>> Coaches { get; private set; }
        public static Dictionary<Guid, List<DbService>> Services { get; private set; }
        public static Dictionary<Guid, List<DbCustomer>> Customers { get; private set; }
        public static Dictionary<Guid, List<DbSingleSession>> Sessions { get; private set; }
        public static Dictionary<Guid, List<DbRepeatedSession>> Courses { get; private set; }
        public static Dictionary<Guid, List<DbSingleSessionBooking>> SessionBookings { get; private set; }
        public static Dictionary<Guid, List<DbCourseBooking>> CourseBookings { get; private set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<DbBusiness>();

            Locations = new Dictionary<Guid, List<DbLocation>>();
            Coaches = new Dictionary<Guid, List<DbCoach>>();
            Services = new Dictionary<Guid, List<DbService>>();
            Customers = new Dictionary<Guid, List<DbCustomer>>();
            Sessions = new Dictionary<Guid, List<DbSingleSession>>();
            Courses = new Dictionary<Guid, List<DbRepeatedSession>>();
            SessionBookings = new Dictionary<Guid, List<DbSingleSessionBooking>>();
            CourseBookings = new Dictionary<Guid, List<DbCourseBooking>>();
        }

        public static void Clear()
        {
            Businesses.Clear();

            Locations.Clear();
            Coaches.Clear();
            Services.Clear();
            Customers.Clear();
            Sessions.Clear();
            Courses.Clear();
            SessionBookings.Clear();
            CourseBookings.Clear();
        }

        public void ClearWasCalled()
        {
            WasGetBusinessByIdCalled = false;
            WasAddBusinessCalled = false;
            WasUpdateBusinessCalled = false;
            WasAddLocationCalled = false;
            WasUpdateLocationCalled = false;
            WasAddCoachCalled = false;
            WasUpdateCoachCalled = false;
            WasGetSessionCalled = false;
            WasGetAllCustomerBookingsCalled = false;
            WasSetBookingPaymentStatusCalled = false;
            WasSetBookingAttendanceCalled = false;
        }


        public BusinessData GetBusiness(Guid businessId)
        {
            WasGetBusinessByIdCalled = true;

            var business = Businesses.FirstOrDefault(x => x.Id == businessId);
            return ConvertToBusinessData(business);
        }

        public BusinessData GetBusiness(string domain)
        {
            var business = Businesses.FirstOrDefault(x => x.Domain == domain);
            return ConvertToBusinessData(business);
        }

        private BusinessData ConvertToBusinessData(DbBusiness business)
        {
            if (business == null)
                return null;

            var businessData = Mapper.Map<DbBusiness, BusinessData>(business);

            businessData.Payment.Currency = business.Currency;
            businessData.Payment.IsOnlinePaymentEnabled = business.IsOnlinePaymentEnabled;
            businessData.Payment.ForceOnlinePayment = business.ForceOnlinePayment;
            businessData.Payment.PaymentProvider = business.PaymentProvider;
            businessData.Payment.MerchantAccountIdentifier = business.MerchantAccountIdentifier;

            return businessData;
        }

        public void AddBusiness(NewBusiness business)
        {
            WasAddBusinessCalled = true;
            DataPassedIn = business;

            var dbBusiness = DbBusinessConverter.Convert(business);
            Businesses.Add(dbBusiness);
        }

        public void UpdateBusiness(Business business)
        {
            WasUpdateBusinessCalled = true;
            DataPassedIn = business;

            var dbNewBusiness = DbBusinessConverter.Convert(business);

            var index = Businesses.FindIndex(x => x.Id == business.Id);
            var dbExistingBusiness = Businesses[index];
            dbNewBusiness.Domain = dbExistingBusiness.Domain;

            Businesses[index] = dbNewBusiness;
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

        public void AddLocation(Guid businessId, Location location)
        {
            WasAddLocationCalled = true;
            BusinessIdPassedIn = businessId;
            DataPassedIn = location;

            var dbLocation = Mapper.Map<Location, DbLocation>(location);

            var dbLocations = GetAllDbLocations(businessId);
            dbLocations.Add(dbLocation);

            Locations[businessId] = dbLocations;
        }

        public void UpdateLocation(Guid businessId, Location location)
        {
            WasUpdateLocationCalled = true;
            BusinessIdPassedIn = businessId;
            DataPassedIn = location;

            var dbLocation = Mapper.Map<Location, DbLocation>(location);

            var dbLocations = GetAllDbLocations(businessId);
            var index = dbLocations.FindIndex(x => x.Id == location.Id);
            dbLocations[index] = dbLocation;
            Locations[businessId] = dbLocations;
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

        public void AddCoach(Guid businessId, Coach coach)
        {
            WasAddCoachCalled = true;
            BusinessIdPassedIn = businessId;
            DataPassedIn = coach;

            var dbCoach = Mapper.Map<Coach, DbCoach>(coach);

            var dbCoaches = GetAllDbCoaches(businessId);
            dbCoaches.Add(dbCoach);

            Coaches[businessId] = dbCoaches;
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

        public void AddService(Guid businessId, Service service)
        {
            var dbService = Mapper.Map<Service, DbService>(service);

            var dbServices = GetAllDbServices(businessId);
            dbServices.Add(dbService);

            Services[businessId] = dbServices;
        }

        public void UpdateService(Guid businessId, Service service)
        {
            var dbService = Mapper.Map<Service, DbService>(service);

            var dbServices = GetAllDbServices(businessId);
            var index = dbServices.FindIndex(x => x.Id == service.Id);
            dbServices[index] = dbService;
            Services[businessId] = dbServices;
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
                session.Service.Name =  service.Name;
            }

            return sessions;
        }

        public SingleSessionData GetSession(Guid businessId, Guid sessionId)
        {
            WasGetSessionCalled = true;

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

        public void DeleteSession(Guid businessId, Guid sessionId)
        {
            var dbSessions = GetAllDbSessions(businessId);
            var dbSession = dbSessions.Find(x => x.Id == sessionId);
            if (dbSession == null)
            {
                // Delete session in course
                var dbCourses = GetAllDbCourses(businessId);
                foreach (var dbCourse in dbCourses)
                {
                    var index = dbCourse.Sessions.ToList().FindIndex(x => x.Id == sessionId);
                    if (index == -1)
                        continue;

                    dbCourse.Sessions.RemoveAt(index);
                }

                Courses[businessId] = dbCourses;
                return;
            }

            dbSessions.Remove(dbSession);

            Sessions[businessId] = dbSessions;
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

            for(var i = 0; i < course.Sessions.Count; i++)
            {
                course.Sessions[i] = GetSession(businessId, course.Sessions[i].Id);
            }

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

        public RepeatedSessionData UpdateCourse(Guid businessId, RepeatedSession course)
        {
            var dbCourse = Mapper.Map<RepeatedSession, DbRepeatedSession>(course);

            var dbCourses = GetAllDbCourses(businessId);
            var index = dbCourses.FindIndex(x => x.Id == course.Id);

            dbCourses[index] = dbCourse;
            Courses[businessId] = dbCourses;

            return GetCourse(businessId, course.Id);
        }

        public void DeleteCourse(Guid businessId, Guid courseId)
        {
            var dbCourses = GetAllDbCourses(businessId);

            var dbCourse = dbCourses.Find(x => x.Id == courseId);

            dbCourses.Remove(dbCourse);

            Courses[businessId] = dbCourses;
        }


        //private IList<BookingData> GetAllBookings(Guid businessId)
        //{
        //    var dbBookings = GetAllDbBookings(businessId);

        //    return Mapper.Map<IList<DbBooking>, IList<BookingData>>(dbBookings);
        //}

        //public BookingData GetBooking(Guid businessId, Guid bookingId)
        //{
        //    var businessBookings = GetAllBookings(businessId);

        //    var booking = businessBookings.FirstOrDefault(x => x.Id == bookingId);
        //    if (booking == null)
        //        return null;

        //    var session = GetSession(businessId, booking.Session.Id);
        //    booking.Session.Name = string.Format("{0} at {1} with {2} on {3} at {4}",
        //                                         session.Service.Name,
        //                                         session.Location.Name,
        //                                         session.Coach.Name,
        //                                         session.Timing.StartDate,
        //                                         session.Timing.StartTime);

        //    var customer = GetCustomer(businessId, booking.Customer.Id);
        //    booking.Customer.Name = string.Format("{0} {1}", customer.FirstName, customer.LastName);

        //    return booking;
        //}

        public SingleSessionBookingData GetSessionBooking(Guid businessId, Guid sessionBookingId)
        {
            var dbSessionBookings = GetAllDbSessionBookings(businessId);
            var dbSessionBooking = dbSessionBookings.SingleOrDefault(x => x.Id == sessionBookingId);

            return Mapper.Map<DbSingleSessionBooking, SingleSessionBookingData>(dbSessionBooking);
        }

        public CourseBookingData GetCourseBooking(Guid businessId, Guid courseBookingId)
        {
            var dbCourseBookings = GetAllDbCourseBookings(businessId);
            var dbCourseBooking = dbCourseBookings.SingleOrDefault(x => x.Id == courseBookingId);

            return Mapper.Map<DbCourseBooking, CourseBookingData>(dbCourseBooking);
        }


        public SingleSessionBookingData AddSessionBooking(Guid businessId, SingleSessionBooking booking)
        {
            var dbBooking = Mapper.Map<SingleSessionBooking, DbSingleSessionBooking>(booking);

            var dbBookings = GetAllDbSessionBookings(businessId);
            dbBookings.Add(dbBooking);

            SessionBookings[businessId] = dbBookings;

            return GetSessionBooking(businessId, booking.Id);
        }

        public CourseBookingData AddCourseBooking(Guid businessId, CourseBooking booking)
        {
            var dbBooking = Mapper.Map<CourseBooking, DbCourseBooking>(booking);

            var dbBookings = GetAllDbCourseBookings(businessId);
            dbBookings.Add(dbBooking);

            CourseBookings[businessId] = dbBookings;

            return GetCourseBooking(businessId, booking.Id);
        }

        public void SetBookingPaymentStatus(Guid businessId, Guid bookingId, string paymentStatus)
        {
            WasSetBookingPaymentStatusCalled = true;

            var dbSessionBookings = GetAllDbSessionBookings(businessId);
            var dbSessionBooking = dbSessionBookings.SingleOrDefault(x => x.Id == bookingId);
            if (dbSessionBooking != null)
            {
                dbSessionBooking.PaymentStatus = paymentStatus;
                return;
            }

            var dbCourseBookings = GetAllDbCourseBookings(businessId);
            var dbCourseBooking = dbCourseBookings.SingleOrDefault(x => x.Id == bookingId);
            if (dbCourseBooking != null)
            {
                dbCourseBooking.PaymentStatus = paymentStatus;
                foreach (var sessionBooking in dbCourseBooking.SessionBookings)
                    sessionBooking.PaymentStatus = paymentStatus;
                return;
            }

            throw new InvalidOperationException("Not a session nor a course booking.");
        }

        public void SetBookingAttendance(Guid businessId, Guid sessionBookingId, bool? hasAttended)
        {
            WasSetBookingAttendanceCalled = true;

            var dbSessionBookings = GetAllDbSessionBookings(businessId);
            var dbSessionBooking = dbSessionBookings.SingleOrDefault(x => x.Id == sessionBookingId);
            if (dbSessionBooking != null)
                dbSessionBooking.HasAttended = hasAttended;
        }

        public void DeleteBooking(Guid businessId, Guid bookingId)
        {
            WasDeleteBookingCalled = true;

            //var dbBookings = GetAllDbBookings(businessId);

            //var dbBooking = dbBookings.Find(x => x.Id == bookingId);

            //dbBookings.Remove(dbBooking);

            //Bookings[businessId] = dbBookings;
        }


        public IList<CustomerBookingData> GetCustomerBookingsBySessionId(Guid businessId, Guid sessionId)
        {
            //var bookings = GetAllBookings(businessId);
            //var sessionBookings = bookings.Where(x => x.Session.Id == sessionId);

            //return sessionBookings.Select(sessionBooking => new CustomerBookingData
            //{
            //    Id = sessionBooking.Id, 
            //    Customer = GetCustomer(businessId, sessionBooking.Customer.Id)
            //}).ToList();
            return null;
        }

        public IList<CustomerBookingData> GetCustomerBookingsByCourseId(Guid businessId, Guid courseId)
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

        private List<DbSingleSessionBooking> GetAllDbSessionBookings(Guid businessId)
        {
            List<DbSingleSessionBooking> businessBookings;
            return SessionBookings.TryGetValue(businessId, out businessBookings)
                ? businessBookings
                : new List<DbSingleSessionBooking>();
        }

        private List<DbCourseBooking> GetAllDbCourseBookings(Guid businessId)
        {
            List<DbCourseBooking> businessBookings;
            return CourseBookings.TryGetValue(businessId, out businessBookings)
                ? businessBookings
                : new List<DbCourseBooking>();
        }


        public IList<CustomerBookingData> GetAllCustomerBookings(Guid businessId)
        {
            WasGetAllCustomerBookingsCalled = true;

            var customerBookings = new List<CustomerBookingData>();
            var customers = GetAllCustomers(businessId);

            // Sessions
            var dbSessionBookings = GetAllDbSessionBookings(businessId);
            foreach (var dbSessionBooking in dbSessionBookings)
            {
                var booking = Mapper.Map<DbSingleSessionBooking, CustomerBookingData>(dbSessionBooking);
                booking.Customer = customers.SingleOrDefault(x => x.Id == booking.Customer.Id);

                customerBookings.Add(booking);
            }

            // Courses
            var dbCourseBookings = GetAllDbCourseBookings(businessId);
            
            // ...


            return customerBookings;
        }


        public IList<EmailTemplateData> GetAllEmailTemplates(Guid businessId)
        {
            throw new NotImplementedException();
        }

        public EmailTemplateData GetEmailTemplate(Guid businessId, string templateType)
        {
            throw new NotImplementedException();
        }

        public void AddEmailTemplate(Guid businessId, EmailTemplate emailTemplate)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmailTemplate(Guid businessId, EmailTemplate emailTemplate)
        {
            throw new NotImplementedException();
        }

        public void DeleteEmailTemplate(Guid businessId, string templateType)
        {
            throw new NotImplementedException();
        }
    }
}