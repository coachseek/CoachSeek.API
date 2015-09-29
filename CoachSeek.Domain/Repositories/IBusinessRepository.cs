using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Entities.EmailTemplating;

namespace CoachSeek.Domain.Repositories
{
    public interface IBusinessRepository
    {
        Task<BusinessData> GetBusinessAsync(Guid businessId);
        Task<BusinessData> GetBusinessAsync(string domain);
        BusinessData GetBusiness(string domain);
        Task AddBusinessAsync(NewBusiness business);
        Task UpdateBusinessAsync(Business business);
        Task SetAuthorisedUntilAsync(Guid businessId, DateTime authorisedUntil);

        Task<IList<LocationData>> GetAllLocationsAsync(Guid businessId);
        IList<LocationData> GetAllLocations(Guid businessId);
        LocationData GetLocation(Guid businessId, Guid locationId);
        void AddLocation(Guid businessId, Location location);
        void UpdateLocation(Guid businessId, Location location);

        Task<IList<CoachData>> GetAllCoachesAsync(Guid businessId);
        IList<CoachData> GetAllCoaches(Guid businessId);
        CoachData GetCoach(Guid businessId, Guid coachId);
        void AddCoach(Guid businessId, Coach coach);
        void UpdateCoach(Guid businessId, Coach coach);

        Task<IList<ServiceData>> GetAllServicesAsync(Guid businessId);
        IList<ServiceData> GetAllServices(Guid businessId);
        ServiceData GetService(Guid businessId, Guid serviceId);
        void AddService(Guid businessId, Service service);
        void UpdateService(Guid businessId, Service service);

        Task<IList<CustomerData>> GetAllCustomersAsync(Guid businessId);
        IList<CustomerData> GetAllCustomers(Guid businessId);
        CustomerData GetCustomer(Guid businessId, Guid customerId);
        void AddCustomer(Guid businessId, Customer customer);
        void UpdateCustomer(Guid businessId, Customer customer);

        Task<IList<SingleSessionData>> SearchForSessions(Guid businessId, string beginDate, string endDate);
        IList<SingleSessionData> GetAllSessions(Guid businessId);
        SingleSessionData GetSession(Guid businessId, Guid sessionId);
        void AddSession(Guid businessId, StandaloneSession session);
        void UpdateSession(Guid businessId, SingleSession session);
        void DeleteSession(Guid businessId, Guid sessionId);

        IList<RepeatedSessionData> GetAllCourses(Guid businessId);
        RepeatedSessionData GetCourse(Guid businessId, Guid courseId);
        void AddCourse(Guid businessId, RepeatedSession course);
        void UpdateCourse(Guid businessId, RepeatedSession course);
        void DeleteCourse(Guid businessId, Guid courseId);

        SingleSessionBookingData GetSessionBooking(Guid businessId, Guid sessionBookingId);
        SingleSessionBookingData AddSessionBooking(Guid businessId, SingleSessionBooking sessionBooking);

        CourseBookingData GetCourseBooking(Guid businessId, Guid courseBookingId);
        CourseBookingData AddCourseBooking(Guid businessId, CourseBooking courseBooking);
        //void UpdateBooking(Guid businessId, BookingData booking);
        void DeleteBooking(Guid businessId, Guid bookingId);

        void SetBookingPaymentStatus(Guid businessId, Guid bookingId, string paymentStatus);
        void SetBookingAttendance(Guid businessId, Guid sessionBookingId, bool? hasAttended);

        IList<CustomerBookingData> GetCustomerBookingsBySessionId(Guid businessId, Guid sessionId);
        IList<CustomerBookingData> GetCustomerBookingsByCourseId(Guid businessId, Guid courseId);
        IList<CustomerBookingData> GetAllCustomerBookings(Guid businessId);

        IList<EmailTemplateData> GetAllEmailTemplates(Guid businessId);
        EmailTemplateData GetEmailTemplate(Guid businessId, string templateType);
        void AddEmailTemplate(Guid businessId, EmailTemplate emailTemplate);
        void UpdateEmailTemplate(Guid businessId, EmailTemplate emailTemplate);
        void DeleteEmailTemplate(Guid businessId, string templateType);
    }
}
