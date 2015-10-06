using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.EmailTemplating;
using CoachSeek.Domain.Repositories;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbBusinessRepository : IBusinessRepository, ITransactionRepository
    {
        private SqlConnection _connection;

        protected virtual string ConnectionStringKey { get { return "BusinessDatabase"; } } 

        private SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
                    _connection = new SqlConnection(connectionString);
                }

                return _connection;
            }
        }

        private DbBusinessEntityRepository BusinessRepository { get; set; }
        private DbLocationRepository LocationRepository { get; set; }
        private DbCoachRepository CoachRepository { get; set; }
        private DbServiceRepository ServiceRepository { get; set; }
        private DbCustomerRepository CustomerRepository { get; set; }
        private DbSessionRepository SessionRepository { get; set; }
        private DbCourseRepository CourseRepository { get; set; }
        private DbBookingRepository BookingRepository { get; set; }
        private DbEmailTemplateRepository EmailTemplateRepository { get; set; }
        private DbTransactionRepository TransactionRepository { get; set; }


        public DbBusinessRepository()
        {
            BusinessRepository = new DbBusinessEntityRepository(ConnectionStringKey);
            LocationRepository = new DbLocationRepository(ConnectionStringKey);
            CoachRepository = new DbCoachRepository(ConnectionStringKey);
            ServiceRepository = new DbServiceRepository(ConnectionStringKey);
            CustomerRepository = new DbCustomerRepository(ConnectionStringKey);
            SessionRepository = new DbSessionRepository(ConnectionStringKey);
            CourseRepository = new DbCourseRepository(ConnectionStringKey);
            BookingRepository = new DbBookingRepository(ConnectionStringKey);
            EmailTemplateRepository = new DbEmailTemplateRepository(ConnectionStringKey);
            TransactionRepository = new DbTransactionRepository(ConnectionStringKey);
        }


        public void CloseConnection()
        {
            if (Connection != null)
                Connection.Close();
        }

        public async Task<BusinessData> GetBusinessAsync(Guid businessId)
        {
            return await BusinessRepository.GetBusinessAsync(businessId);
        }

        public async Task<BusinessData> GetBusinessAsync(string domain)
        {
            return await BusinessRepository.GetBusinessAsync(domain);
        }

        public async Task AddBusinessAsync(NewBusiness business)
        {
            await BusinessRepository.AddBusinessAsync(business);
        }

        public async Task UpdateBusinessAsync(Business business)
        {
            await BusinessRepository.UpdateBusinessAsync(business);
        }

        public async Task SetAuthorisedUntilAsync(Guid businessId, DateTime authorisedUntil)
        {
            await BusinessRepository.SetAuthorisedUntilAsync(businessId, authorisedUntil);
        }


        public async Task<IList<LocationData>> GetAllLocationsAsync(Guid businessId)
        {
            return await LocationRepository.GetAllLocationsAsync(businessId);
        }

        public IList<LocationData> GetAllLocations(Guid businessId)
        {
            return LocationRepository.GetAllLocations(businessId);
        }

        public LocationData GetLocation(Guid businessId, Guid locationId)
        {
            return LocationRepository.GetLocation(businessId, locationId);
        }

        public void AddLocation(Guid businessId, Location location)
        {
            LocationRepository.AddLocation(businessId, location);
        }

        public void UpdateLocation(Guid businessId, Location location)
        {
            LocationRepository.UpdateLocation(businessId, location);
        }


        public async Task<IList<CoachData>> GetAllCoachesAsync(Guid businessId)
        {
            return await CoachRepository.GetAllCoachesAsync(businessId);
        }

        public IList<CoachData> GetAllCoaches(Guid businessId)
        {
            return CoachRepository.GetAllCoaches(businessId);
        }

        public CoachData GetCoach(Guid businessId, Guid coachId)
        {
            return CoachRepository.GetCoach(businessId, coachId);
        }

        public void AddCoach(Guid businessId, Coach coach)
        {
            CoachRepository.AddCoach(businessId, coach);
        }

        public void UpdateCoach(Guid businessId, Coach coach)
        {
            CoachRepository.UpdateCoach(businessId, coach);
        }


        public async Task<IList<ServiceData>> GetAllServicesAsync(Guid businessId)
        {
            return await ServiceRepository.GetAllServicesAsync(businessId);
        }

        public IList<ServiceData> GetAllServices(Guid businessId)
        {
            return ServiceRepository.GetAllServices(businessId);
        }

        public ServiceData GetService(Guid businessId, Guid serviceId)
        {
            return ServiceRepository.GetService(businessId, serviceId);
        }

        public void AddService(Guid businessId, Service service)
        {
            ServiceRepository.AddService(businessId, service);
        }

        public void UpdateService(Guid businessId, Service service)
        {
            ServiceRepository.UpdateService(businessId, service);
        }


        public async Task<IList<CustomerData>> GetAllCustomersAsync(Guid businessId)
        {
            return await CustomerRepository.GetAllCustomersAsync(businessId);
        }

        public async Task<CustomerData> GetCustomerAsync(Guid businessId, Guid customerId)
        {
            return await CustomerRepository.GetCustomerAsync(businessId, customerId);
        }

        public CustomerData GetCustomer(Guid businessId, Guid customerId)
        {
            return CustomerRepository.GetCustomer(businessId, customerId);
        }

        public async Task AddCustomerAsync(Guid businessId, Customer customer)
        {
            await CustomerRepository.AddCustomerAsync(businessId, customer);
        }

        public async Task UpdateCustomerAsync(Guid businessId, Customer customer)
        {
            await CustomerRepository.UpdateCustomerAsync(businessId, customer);
        }


        public async Task<IList<SingleSessionData>> SearchForSessions(Guid businessId, string beginDate, string endDate)
        {
            return await SessionRepository.SearchForSessionsAsync(businessId, beginDate, endDate);
        }

        public async Task<IList<SingleSessionData>> GetAllSessionsAsync(Guid businessId)
        {
            return await SessionRepository.GetAllSessionsAsync(businessId);
        }

        public IList<SingleSessionData> GetAllSessions(Guid businessId)
        {
            return SessionRepository.GetAllSessions(businessId);
        }

        public SingleSessionData GetSession(Guid businessId, Guid sessionId)
        {
            return SessionRepository.GetSession(businessId, sessionId);
        }

        public void AddSession(Guid businessId, StandaloneSession session)
        {
            SessionRepository.AddSession(businessId, session);
        }

        public void UpdateSession(Guid businessId, SingleSession session)
        {
            SessionRepository.UpdateSession(businessId, session);
        }

        public void DeleteSession(Guid businessId, Guid sessionId)
        {
            SessionRepository.DeleteSession(businessId, sessionId);
        }

        public async Task<IList<RepeatedSessionData>> GetAllCoursesAsync(Guid businessId)
        {
            return await CourseRepository.GetAllCoursesAsync(businessId);
        }

        public IList<RepeatedSessionData> GetAllCourses(Guid businessId)
        {
            return CourseRepository.GetAllCourses(businessId);
        }

        public RepeatedSessionData GetCourse(Guid businessId, Guid courseId)
        {
            return CourseRepository.GetCourse(businessId, courseId);
        }

        public void AddCourse(Guid businessId, RepeatedSession course)
        {
            CourseRepository.AddCourse(businessId, course);
        }

        public void UpdateCourse(Guid businessId, RepeatedSession course)
        {
            CourseRepository.UpdateCourse(businessId, course);
        }

        public void DeleteCourse(Guid businessId, Guid courseId)
        {
            CourseRepository.DeleteCourse(businessId, courseId);
        }


        public SingleSessionBookingData GetSessionBooking(Guid businessId, Guid sessionBookingId)
        {
            return BookingRepository.GetSessionBooking(businessId, sessionBookingId);
        }

        public SingleSessionBookingData AddSessionBooking(Guid businessId, SingleSessionBooking booking)
        {
            return BookingRepository.AddSessionBooking(businessId, booking);
        }

        public CourseBookingData AddCourseBooking(Guid businessId, CourseBooking courseBooking)
        {
            return BookingRepository.AddCourseBooking(businessId, courseBooking);
        }
        
        public CourseBookingData GetCourseBooking(Guid businessId, Guid courseBookingId)
        {
            return BookingRepository.GetCourseBooking(businessId, courseBookingId);
        }

        public void DeleteBooking(Guid businessId, Guid bookingId)
        {
            BookingRepository.DeleteBooking(businessId, bookingId);
        }

        public void SetBookingPaymentStatus(Guid businessId, Guid bookingId, string paymentStatus)
        {
            BookingRepository.SetBookingPaymentStatus(businessId, bookingId, paymentStatus);
        }

        public void SetBookingAttendance(Guid businessId, Guid sessionBookingId, bool? hasAttended)
        {
            BookingRepository.SetBookingAttendance(businessId, sessionBookingId, hasAttended);
        }

        public async Task<IList<CustomerBookingData>> GetAllCustomerBookingsAsync(Guid businessId)
        {
            return await BookingRepository.GetAllCustomerBookingsAsync(businessId);
        }

        public IList<CustomerBookingData> GetAllCustomerBookings(Guid businessId)
        {
            return BookingRepository.GetAllCustomerBookings(businessId);
        }

        public IList<CustomerBookingData> GetCustomerBookingsBySessionId(Guid businessId, Guid sessionId)
        {
            return BookingRepository.GetCustomerBookingsBySessionId(businessId, sessionId);
        }

        public IList<CustomerBookingData> GetCustomerBookingsByCourseId(Guid businessId, Guid courseId)
        {
            return BookingRepository.GetCustomerBookingsByCourseId(businessId, courseId);
        }


        public IList<EmailTemplateData> GetAllEmailTemplates(Guid businessId)
        {
            return EmailTemplateRepository.GetAllEmailTemplates(businessId);
        }

        public EmailTemplateData GetEmailTemplate(Guid businessId, string templateType)
        {
            return EmailTemplateRepository.GetEmailTemplate(businessId, templateType);
        }

        public void AddEmailTemplate(Guid businessId, EmailTemplate emailTemplate)
        {
            EmailTemplateRepository.AddEmailTemplate(businessId, emailTemplate);
        }

        public void UpdateEmailTemplate(Guid businessId, EmailTemplate emailTemplate)
        {
            EmailTemplateRepository.UpdateEmailTemplate(businessId, emailTemplate);
        }

        public void DeleteEmailTemplate(Guid businessId, string templateType)
        {
            EmailTemplateRepository.DeleteEmailTemplate(businessId, templateType);
        }


        public Payment GetPayment(string paymentProvider, string id)
        {
            return TransactionRepository.GetPayment(paymentProvider, id);
        }

        public void AddPayment(NewPayment payment)
        {
            TransactionRepository.AddPayment(payment);
        }
    }
}
