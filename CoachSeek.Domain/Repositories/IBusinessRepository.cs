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
        BusinessData GetBusiness(Guid businessId);
        BusinessData GetBusiness(string domain);
        void AddBusiness(NewBusiness business);
        void UpdateBusiness(Business business);
        void SetAuthorisedUntil(Guid businessId, DateTime authorisedUntil);

        Task<IList<LocationData>> GetAllLocationsAsync(Guid businessId);
        IList<LocationData> GetAllLocations(Guid businessId);
        LocationData GetLocation(Guid businessId, Guid locationId);
        void AddLocation(Guid businessId, Location location);
        void UpdateLocation(Guid businessId, Location location);

        IList<CoachData> GetAllCoaches(Guid businessId);
        CoachData GetCoach(Guid businessId, Guid coachId);
        void AddCoach(Guid businessId, Coach coach);
        void UpdateCoach(Guid businessId, Coach coach);

        IList<ServiceData> GetAllServices(Guid businessId);
        ServiceData GetService(Guid businessId, Guid serviceId);
        void AddService(Guid businessId, Service service);
        void UpdateService(Guid businessId, Service service);

        IList<CustomerData> GetAllCustomers(Guid businessId);
        CustomerData GetCustomer(Guid businessId, Guid customerId);
        void AddCustomer(Guid businessId, Customer customer);
        void UpdateCustomer(Guid businessId, Customer customer);

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
