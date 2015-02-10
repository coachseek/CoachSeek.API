using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities.Booking
{
    public class Business
    {
        public Guid BusinessId { get; protected set; }
        //public string Name { get; protected set; }
        //public string Domain { get; protected set; }
        //public IList<LocationData> Locations { get { return BusinessLocations.ToData(); } }
        //public IList<CoachData> Coaches { get { return BusinessCoaches.ToData(); } }
        //public IList<ServiceData> Services { get { return BusinessServices.ToData(); } }
        //public IList<SessionData> Sessions { get { return BusinessSessions.ToData(); } }
        public IList<CustomerData> Customers { get { return BusinessCustomers.ToData(); } }
        //public IList<BookingData> Bookings { get { return BusinessBookings.ToData(); } }

        //private BusinessLocations BusinessLocations { get; set; }
        //private BusinessCoaches BusinessCoaches { get; set; }
        //private BusinessServices BusinessServices { get; set; }
        //private BusinessSessions BusinessSessions { get; set; }
        private BusinessCustomers BusinessCustomers { get; set; }
        private BusinessBookings BusinessBookings { get; set; }

        public Business(Guid businessId)
        {
            BusinessId = businessId;
        }

        //public Business(Guid id, 
        //    string name, 
        //    string domain, 
        //    IEnumerable<LocationData> locations, 
        //    IEnumerable<CoachData> coaches,
        //    IEnumerable<ServiceData> services,
        //    IEnumerable<SessionData> sessions,
        //    IEnumerable<CustomerData> customers
        //    )
        //{
        //    Id = id;
        //    Name = name;
        //    Domain = domain;
        //    BusinessLocations = new BusinessLocations(locations);
        //    BusinessCoaches = new BusinessCoaches(coaches);
        //    BusinessServices = new BusinessServices(services);
        //    BusinessSessions = new BusinessSessions(sessions, BusinessLocations, BusinessCoaches, BusinessServices);
        //    BusinessCustomers = new BusinessCustomers(customers);
        //}

        //public Business()
        //{
        //    Id = Guid.NewGuid();
        //    BusinessLocations = new BusinessLocations();
        //    BusinessCoaches = new BusinessCoaches();
        //    BusinessServices = new BusinessServices();
        //    BusinessSessions = new BusinessSessions();
        //    BusinessCustomers = new BusinessCustomers();
        //}

        //// Minimal Unit testing constructor.
        //public Business(Guid id)
        //    : this()
        //{
        //    Id = id;
        //}



        //public LocationData AddLocation(LocationAddCommand command, IBusinessRepository businessRepository)
        //{
        //    var locationId = BusinessLocations.Add(command.ToData());
        //    businessRepository.Save(this);

        //    return GetLocationById(locationId, businessRepository);
        //}

        //public LocationData UpdateLocation(LocationUpdateCommand command, IBusinessRepository businessRepository)
        //{
        //    BusinessLocations.Update(command.ToData());
        //    businessRepository.Save(this);

        //    return GetLocationById(command.Id, businessRepository);
        //}

        //public CoachData AddCoach(CoachAddCommand command, IBusinessRepository businessRepository)
        //{
        //    var coachId = BusinessCoaches.Add(command.ToData());
        //    businessRepository.Save(this);

        //    return GetCoachById(coachId, businessRepository);
        //}

        //public CoachData UpdateCoach(CoachUpdateCommand command, IBusinessRepository businessRepository)
        //{
        //    BusinessCoaches.Update(command.ToData());
        //    businessRepository.Save(this);

        //    return GetCoachById(command.Id, businessRepository);
        //}

        //public ServiceData AddService(ServiceAddCommand command, IBusinessRepository businessRepository)
        //{
        //    var serviceId = BusinessServices.Add(command.ToData());
        //    businessRepository.Save(this);

        //    return GetServiceById(serviceId, businessRepository);
        //}

        //public ServiceData UpdateService(ServiceUpdateCommand command, IBusinessRepository businessRepository)
        //{
        //    BusinessServices.Update(command.ToData());
        //    businessRepository.Save(this);

        //    return GetServiceById(command.Id, businessRepository);
        //}

        //public CustomerData AddCustomer(CustomerAddCommand command, IBusinessRepository businessRepository)
        //{
        //    var customerId = BusinessCustomers.Add(command.ToData());
        //    businessRepository.Save(this);

        //    return GetCustomerById(customerId, businessRepository);
        //}

        //public CustomerData UpdateCustomer(CustomerUpdateCommand command, IBusinessRepository businessRepository)
        //{
        //    BusinessCustomers.Update(command.ToData());
        //    businessRepository.Save(this);

        //    return GetCustomerById(command.Id, businessRepository);
        //}

        //public SessionData AddSession(SessionAddCommand command, IBusinessRepository businessRepository)
        //{
        //    var location = GetLocationById(command.Location.Id, businessRepository);
        //    var coach = GetCoachById(command.Coach.Id, businessRepository);
        //    var service = GetServiceById(command.Service.Id, businessRepository);

        //    var sessionId = BusinessSessions.Add(command.ToData(), location, coach, service);
        //    businessRepository.Save(this);

        //    return GetSessionById(sessionId, businessRepository);
        //}

        //public SessionData UpdateSession(SessionUpdateCommand command, IBusinessRepository businessRepository)
        //{
        //    var location = GetLocationById(command.Location.Id, businessRepository);
        //    var coach = GetCoachById(command.Coach.Id, businessRepository);
        //    var service = GetServiceById(command.Service.Id, businessRepository);

        //    BusinessSessions.Update(command.ToData(), location, coach, service);
        //    businessRepository.Save(this);

        //    return GetSessionById(command.Id, businessRepository);
        //}

        public BookingData AddBooking(BookingAddCommand command, IBusinessRepository businessRepository, IBookingRepository bookingRepository)
        {
            var session = GetSessionById(command.Session.Id, businessRepository);
            var customer = GetCustomerById(command.Customer.Id, businessRepository);

            var booking = BusinessBookings.Add(session.ToKeyData(), customer.ToKeyData());
            bookingRepository.SaveNew(BusinessId, booking);

            return GetBookingById(booking.Id, bookingRepository);
        }


        public BusinessData ToData()
        {
            return Mapper.Map<Business, BusinessData>(this);
        }


        //private LocationData GetLocationById(Guid locationId, IBusinessRepository businessRepository)
        //{
        //    var location = businessRepository.Get(Id).Locations.FirstOrDefault(x => x.Id == locationId);
        //    if (location == null)
        //        throw new InvalidLocation();
        //    return location;
        //}

        //private CoachData GetCoachById(Guid coachId, IBusinessRepository businessRepository)
        //{
        //    var coach = businessRepository.Get(Id).Coaches.FirstOrDefault(x => x.Id == coachId);
        //    if (coach == null)
        //        throw new InvalidCoach();
        //    return coach;
        //}

        //private ServiceData GetServiceById(Guid serviceId, IBusinessRepository businessRepository)
        //{
        //    var service = businessRepository.Get(Id).Services.FirstOrDefault(x => x.Id == serviceId);
        //    if (service == null)
        //        throw new InvalidService();
        //    return service;
        //}

        private SessionData GetSessionById(Guid sessionId, IBusinessRepository businessRepository)
        {
            var session = businessRepository.Get(BusinessId).Sessions.FirstOrDefault(x => x.Id == sessionId);
            if (session == null)
                throw new InvalidSession();
            return session;
        }

        private CustomerData GetCustomerById(Guid customerId, IBusinessRepository businessRepository)
        {
            var customer = businessRepository.Get(BusinessId).Customers.FirstOrDefault(x => x.Id == customerId);
            if (customer == null)
                throw new InvalidCustomer();
            return customer;
        }

        private BookingData GetBookingById(Guid bookingId, IBookingRepository bookingRepository)
        {
            var booking = bookingRepository.Get(BusinessId, bookingId);
            if (booking == null)
                throw new InvalidBooking();
            return booking.ToData();
        }
    }
}