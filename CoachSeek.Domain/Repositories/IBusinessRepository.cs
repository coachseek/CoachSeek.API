using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface IBusinessRepository
    {
        BusinessData GetBusiness(Guid businessId);
        BusinessData GetBusiness(string domain);
        void AddBusiness(Business business);
        BusinessData UpdateBusiness(Business business);

        IList<LocationData> GetAllLocations(Guid businessId);
        LocationData GetLocation(Guid businessId, Guid locationId);
        LocationData AddLocation(Guid businessId, Location location);
        LocationData UpdateLocation(Guid businessId, Location location);

        IList<CoachData> GetAllCoaches(Guid businessId);
        CoachData GetCoach(Guid businessId, Guid coachId);
        CoachData AddCoach(Guid businessId, Coach coach);
        CoachData UpdateCoach(Guid businessId, Coach coach);

        IList<ServiceData> GetAllServices(Guid businessId);
        ServiceData GetService(Guid businessId, Guid serviceId);
        ServiceData AddService(Guid businessId, Service service);
        ServiceData UpdateService(Guid businessId, Service service);

        IList<CustomerData> GetAllCustomers(Guid businessId);
        CustomerData GetCustomer(Guid businessId, Guid customerId);
        CustomerData AddCustomer(Guid businessId, Customer customer);
        CustomerData UpdateCustomer(Guid businessId, Customer customer);

        IList<SingleSessionData> GetAllStandaloneSessions(Guid businessId);
        IList<SingleSessionData> GetAllSessions(Guid businessId);
        SingleSessionData GetSession(Guid businessId, Guid sessionId);
        SingleSessionData AddSession(Guid businessId, StandaloneSession session);
        SingleSessionData UpdateSession(Guid businessId, SingleSession session);
        void DeleteSession(Guid businessId, Guid sessionId);

        IList<RepeatedSessionData> GetAllCourses(Guid businessId);
        RepeatedSessionData GetCourse(Guid businessId, Guid courseId);
        RepeatedSessionData AddCourse(Guid businessId, RepeatedSession course);
        RepeatedSessionData UpdateCourse(Guid businessId, RepeatedSession course);
        void DeleteCourse(Guid businessId, Guid courseId);

        SingleSessionBookingData GetSessionBooking(Guid businessId, Guid sessionBookingId);
        SingleSessionBookingData AddSessionBooking(Guid businessId, SingleSessionBooking sessionBooking);

        CourseBookingData GetCourseBooking(Guid businessId, Guid courseBookingId);
        CourseBookingData AddCourseBooking(Guid businessId, CourseBooking courseBooking);
        void UpdateBooking(Guid businessId, BookingData booking);
        void DeleteBooking(Guid businessId, Guid bookingId);

        void SetBookingPaymentStatus(Guid businessId, Guid bookingId, string paymentStatus);

        IList<CustomerBookingData> GetCustomerBookingsBySessionId(Guid businessId, Guid sessionId);
        IList<CustomerBookingData> GetCustomerBookingsByCourseId(Guid businessId, Guid courseId);
        IList<CustomerBookingData> GetAllCustomerBookings(Guid businessId);
    }
}
