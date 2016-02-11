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
        Task AddBusinessAsync(NewBusiness business);
        Task UpdateBusinessAsync(Business business);
        Task SetAuthorisedUntilAsync(Guid businessId, DateTime authorisedUntil);
        Task SetUseProRataPricingAsync(Guid businessId, bool useProRataPricing);

        Task<IList<LocationData>> GetAllLocationsAsync(Guid businessId);
        IList<LocationData> GetAllLocations(Guid businessId);
        Task<LocationData> GetLocationAsync(Guid businessId, Guid locationId);
        LocationData GetLocation(Guid businessId, Guid locationId);
        Task AddLocationAsync(Guid businessId, Location location);
        Task UpdateLocationAsync(Guid businessId, Location location);

        Task<IList<CoachData>> GetAllCoachesAsync(Guid businessId);
        IList<CoachData> GetAllCoaches(Guid businessId);
        Task<CoachData> GetCoachAsync(Guid businessId, Guid coachId);
        CoachData GetCoach(Guid businessId, Guid coachId);
        void AddCoach(Guid businessId, Coach coach);
        void UpdateCoach(Guid businessId, Coach coach);

        Task<IList<ServiceData>> GetAllServicesAsync(Guid businessId);
        IList<ServiceData> GetAllServices(Guid businessId);
        Task<ServiceData> GetServiceAsync(Guid businessId, Guid serviceId);
        ServiceData GetService(Guid businessId, Guid serviceId);
        void AddService(Guid businessId, Service service);
        void UpdateService(Guid businessId, Service service);

        Task<IList<CustomerData>> GetAllCustomersAsync(Guid businessId);
        Task<CustomerData> GetCustomerAsync(Guid businessId, Guid customerId);
        CustomerData GetCustomer(Guid businessId, Guid customerId);
        Task AddCustomerAsync(Guid businessId, Customer customer);
        Task UpdateCustomerAsync(Guid businessId, Customer customer);

        Task AddCustomFieldTemplateAsync(Guid businessId, CustomFieldTemplate template);
        Task UpdateCustomFieldTemplateAsync(Guid businessId, CustomFieldTemplate template);
        Task<CustomFieldTemplateData> GetCustomFieldTemplateAsync(Guid businessId, Guid templateId);
        Task<IList<CustomFieldTemplateData>> GetCustomFieldTemplatesAsync(Guid businessId, string type, string key = null);
        Task DeleteCustomFieldTemplateAsync(Guid businessId, string type, string key);
        Task SetCustomFieldTemplateIsActiveAsync(Guid businessId, Guid templateId, bool isActive);

        Task AddCustomFieldValueAsync(Guid businessId, CustomFieldValue value);
        Task UpdateCustomFieldValueAsync(Guid businessId, CustomFieldValue value);
        Task DeleteCustomFieldValueAsync(Guid businessId, CustomFieldValue value);
        Task<CustomFieldValueData> GetCustomFieldValueAsync(Guid businessId, string type, Guid typeId, string key);
        Task<IList<CustomFieldValueData>> GetCustomFieldValuesByTypeIdAsync(Guid businessId, string type, Guid typeId);
        Task<IList<CustomFieldValueData>> GetCustomFieldValuesByTypeAsync(Guid businessId, string type);

        Task<IList<SingleSessionData>> SearchForSessions(Guid businessId, string beginDate, string endDate);
        Task<IList<SingleSessionData>> GetAllSessionsAsync(Guid businessId);
        IList<SingleSessionData> GetAllSessions(Guid businessId);
        Task<SingleSessionData> GetSessionAsync(Guid businessId, Guid sessionId);
        SingleSessionData GetSession(Guid businessId, Guid sessionId);
        Task AddSessionAsync(Guid businessId, StandaloneSession session);
        void AddSession(Guid businessId, StandaloneSession session);
        void UpdateSession(Guid businessId, SingleSession session);
        Task DeleteSessionAsync(Guid businessId, Guid sessionId);
        void DeleteSession(Guid businessId, Guid sessionId);

        Task<IList<RepeatedSessionData>> GetAllCoursesAsync(Guid businessId);
        IList<RepeatedSessionData> GetAllCourses(Guid businessId);
        Task<RepeatedSessionData> GetCourseAsync(Guid businessId, Guid courseId);
        RepeatedSessionData GetCourse(Guid businessId, Guid courseId);
        Task AddCourseAsync(Guid businessId, RepeatedSession course);
        void AddCourse(Guid businessId, RepeatedSession course);
        void UpdateCourse(Guid businessId, RepeatedSession course);
        void DeleteCourse(Guid businessId, Guid courseId);

        Task<SingleSessionBookingData> GetSessionBookingAsync(Guid businessId, Guid sessionBookingId);
        SingleSessionBookingData GetSessionBooking(Guid businessId, Guid sessionBookingId);
        SingleSessionBookingData AddSessionBooking(Guid businessId, SingleSessionBookingData sessionBooking);

        Task<CourseBookingData> GetCourseBookingAsync(Guid businessId, Guid courseBookingId);
        Task<IList<CourseBookingData>> GetCourseBookingsAsync(Guid businessId, Guid courseId, Guid customerId);
        CourseBookingData GetCourseBooking(Guid businessId, Guid courseBookingId);
        IList<CourseBookingData> GetCourseBookings(Guid businessId, Guid courseId, Guid customerId);
        CourseBookingData AddCourseBooking(Guid businessId, CourseBooking courseBooking);
        //void AppendToCourseBooking(Guid businessId, CourseBooking courseBooking, SingleSessionBooking sessionBooking);
        //void UpdateBooking(Guid businessId, BookingData booking);
        void DeleteBooking(Guid businessId, Guid bookingId);

        Task SetBookingPaymentStatusAsync(Guid businessId, Guid bookingId, string paymentStatus);
        void SetBookingPaymentStatus(Guid businessId, Guid bookingId, string paymentStatus);
        void SetBookingAttendance(Guid businessId, Guid sessionBookingId, bool? hasAttended);

        Task<CustomerBookingData> GetCustomerBookingAsync(Guid businessId, Guid bookingId);
        Task<IList<CustomerBookingData>> GetCustomerBookingsBySessionIdAsync(Guid businessId, Guid sessionId);
        Task<IList<CustomerBookingData>> GetCustomerBookingsByCourseIdAsync(Guid businessId, Guid courseId);
        Task<IList<CustomerBookingData>> GetAllCustomerBookingsAsync(Guid businessId);
        IList<CustomerBookingData> GetAllCustomerBookings(Guid businessId);
        IList<CustomerBookingData> GetAllCustomerSessionBookingsByCustomerId(Guid businessId, Guid customerId);

        IList<EmailTemplateData> GetAllEmailTemplates(Guid businessId);
        EmailTemplateData GetEmailTemplate(Guid businessId, string templateType);
        void AddEmailTemplate(Guid businessId, EmailTemplate emailTemplate);
        void UpdateEmailTemplate(Guid businessId, EmailTemplate emailTemplate);
        void DeleteEmailTemplate(Guid businessId, string templateType);
    }
}
