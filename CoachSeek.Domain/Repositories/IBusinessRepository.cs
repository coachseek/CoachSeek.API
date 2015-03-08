﻿using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using System;
using CoachSeek.Domain.Entities.Booking;

namespace CoachSeek.Domain.Repositories
{
    public interface IBusinessRepository
    {
        BusinessData GetBusiness(Guid businessId);
        BusinessData AddBusiness(Business business);


        bool IsAvailableDomain(string domain);


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

        RepeatedSessionData GetCourse(Guid businessId, Guid courseId);
        RepeatedSessionData AddCourse(Guid businessId, RepeatedSession course);

        IList<BookingData> GetAllBookings(Guid businessId);
        BookingData AddBooking(Guid businessId, Booking booking);
    }
}
