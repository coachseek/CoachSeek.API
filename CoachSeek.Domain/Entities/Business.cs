using System.Linq;
using AutoMapper;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Domain { get; protected set; }
        public IList<LocationData> Locations { get { return BusinessLocations.ToData(); } }
        public IList<CoachData> Coaches { get { return BusinessCoaches.ToData(); } }
        public IList<ServiceData> Services { get { return BusinessServices.ToData(); } }
        public IList<SingleSessionData> Sessions { get { return BusinessSessions.ToData(); } }
        public IList<RepeatedSessionData> Courses { get { return BusinessCourses.ToData(); } }
        public IList<CustomerData> Customers { get { return BusinessCustomers.ToData(); } }

        private BusinessLocations BusinessLocations { get; set; }
        private BusinessCoaches BusinessCoaches { get; set; }
        private BusinessServices BusinessServices { get; set; }
        private BusinessSessions BusinessSessions { get; set; }
        private BusinessCourses BusinessCourses { get; set; }
        private BusinessCustomers BusinessCustomers { get; set; }

        public Booking.Business BookingBusiness { get { return new Booking.Business(Id); } }


        public Business(Guid id, 
            string name, 
            string domain, 
            IEnumerable<LocationData> locations, 
            IEnumerable<CoachData> coaches,
            IEnumerable<ServiceData> services,
            IEnumerable<SingleSessionData> sessions,
            IEnumerable<RepeatedSessionData> courses,
            IEnumerable<CustomerData> customers)
        {
            Id = id;
            Name = name;
            Domain = domain;
            BusinessLocations = new BusinessLocations(locations);
            BusinessCoaches = new BusinessCoaches(coaches);
            BusinessServices = new BusinessServices(services);
            BusinessCustomers = new BusinessCustomers(customers);
            BusinessSessions = new BusinessSessions(sessions, BusinessLocations, BusinessCoaches, BusinessServices);
            BusinessCourses = new BusinessCourses(courses, BusinessLocations, BusinessCoaches, BusinessServices);

            BusinessSessions.Courses = BusinessCourses;
            BusinessCourses.Sessions = BusinessSessions;
        }

        public Business()
        {
            Id = Guid.NewGuid();

            BusinessLocations = new BusinessLocations();
            BusinessCoaches = new BusinessCoaches();
            BusinessServices = new BusinessServices();
            BusinessCustomers = new BusinessCustomers();
            BusinessSessions = new BusinessSessions();
            BusinessCourses = new BusinessCourses();

            BusinessSessions.Courses = BusinessCourses;
            BusinessCourses.Sessions = BusinessSessions;
        }

        // Minimal Unit testing constructor.
        public Business(Guid id)
            : this()
        {
            Id = id;
        }


        public LocationData AddLocation(LocationAddCommand command, IBusinessRepository businessRepository)
        {
            var locationId = BusinessLocations.Add(command.ToData());
            businessRepository.Save(this);

            return GetLocationById(locationId, businessRepository);
        }

        public LocationData UpdateLocation(LocationUpdateCommand command, IBusinessRepository businessRepository)
        {
            BusinessLocations.Update(command.ToData());
            businessRepository.Save(this);

            return GetLocationById(command.Id, businessRepository);
        }

        public CoachData AddCoach(CoachAddCommand command, IBusinessRepository businessRepository)
        {
            var coachId = BusinessCoaches.Add(command.ToData());
            businessRepository.Save(this);

            return GetCoachById(coachId, businessRepository);
        }

        public CoachData UpdateCoach(CoachUpdateCommand command, IBusinessRepository businessRepository)
        {
            BusinessCoaches.Update(command.ToData());
            businessRepository.Save(this);

            return GetCoachById(command.Id, businessRepository);
        }

        public ServiceData AddService(ServiceAddCommand command, IBusinessRepository businessRepository)
        {
            var serviceId = BusinessServices.Add(command.ToData());
            businessRepository.Save(this);

            return GetServiceById(serviceId, businessRepository);
        }

        public ServiceData UpdateService(ServiceUpdateCommand command, IBusinessRepository businessRepository)
        {
            BusinessServices.Update(command.ToData());
            businessRepository.Save(this);

            return GetServiceById(command.Id, businessRepository);
        }

        public CustomerData AddCustomer(CustomerAddCommand command, IBusinessRepository businessRepository)
        {
            var customerId = BusinessCustomers.Add(command.ToData());
            businessRepository.Save(this);

            return GetCustomerById(customerId, businessRepository);
        }

        public CustomerData UpdateCustomer(CustomerUpdateCommand command, IBusinessRepository businessRepository)
        {
            BusinessCustomers.Update(command.ToData());
            businessRepository.Save(this);

            return GetCustomerById(command.Id, businessRepository);
        }

        public SessionData AddSession(SessionAddCommand command, IBusinessRepository businessRepository)
        {
            var location = GetLocationById(command.Location.Id, businessRepository);
            var coach = GetCoachById(command.Coach.Id, businessRepository);
            var service = GetServiceById(command.Service.Id, businessRepository);

            if (IsStandaloneSession(command, service))
            {
                var sessionId = BusinessSessions.Add(command, location, coach, service);
                businessRepository.Save(this);
                return GetSessionById(sessionId, businessRepository);
            }
            else
            {
                var courseId = BusinessCourses.Add(command, location, coach, service);
                businessRepository.Save(this);
                return GetCourseById(courseId, businessRepository);
            }
        }

        public SessionData UpdateSession(SessionUpdateCommand command, IBusinessRepository businessRepository)
        {
            var coreData = LookupCoreData(command, businessRepository);

            var session = GetSessionById(command.Id, businessRepository);
            if (session.IsExisting())
                ValidateOrDefaultSessionRepetition(command);
            else
                ValidateOrDefaultCourseRepetition(command, businessRepository);

            if (IsStandaloneSession(command, coreData.Service))
                return UpdateStandaloneSession(command, businessRepository, coreData);

            return UpdateCourse(command, businessRepository, coreData);
        }

        public BusinessData ToData()
        {
            return Mapper.Map<Business, BusinessData>(this);
        }


        private CoreData LookupCoreData(SessionUpdateCommand command, IBusinessRepository businessRepository)
        {
            var location = GetLocationById(command.Location.Id, businessRepository);
            var coach = GetCoachById(command.Coach.Id, businessRepository);
            var service = GetServiceById(command.Service.Id, businessRepository);

            return new CoreData(location, coach, service);
        }

        private static void ValidateOrDefaultSessionRepetition(SessionUpdateCommand command)
        {
            if (command.Repetition == null)
                command.Repetition = new RepetitionCommand(1);
            else if (command.Repetition.SessionCount != 1)
                throw new ValidationException("Cannot change from a standalone session to a course.");
        }

        private void ValidateOrDefaultCourseRepetition(SessionUpdateCommand command, IBusinessRepository businessRepository)
        {
            var course = GetCourseById(command.Id, businessRepository);

            if (course.IsExisting())
            {
                if (command.Repetition == null)
                    command.Repetition = new RepetitionCommand(course.Repetition.SessionCount, 
                                                               course.Repetition.RepeatFrequency);
                else if (!IsMatchingExistingRepetition(command.Repetition, course.Repetition))
                    throw new ValidationException("Cannot change the repetition of a course.");
            }
        }

        private bool IsMatchingExistingRepetition(RepetitionCommand newRepetition, RepetitionData existingRepetition)
        {
            return newRepetition.SessionCount == existingRepetition.SessionCount
                   && newRepetition.RepeatFrequency == existingRepetition.RepeatFrequency;
        }

        private SessionData UpdateStandaloneSession(SessionUpdateCommand command, IBusinessRepository businessRepository, CoreData coreData)
        {
            BusinessSessions.Update(command, coreData.Location, coreData.Coach, coreData.Service);
            businessRepository.Save(this);
            return GetSessionById(command.Id, businessRepository);
        }

        private SessionData UpdateCourse(SessionUpdateCommand command, IBusinessRepository businessRepository, CoreData coreData)
        {
            throw new ValidationException("Course updates are not working yet.");

            //BusinessCourses.Update(command, coreData.Location, coreData.Coach, coreData.Service);
            //businessRepository.Save(this);
            //return GetCourseById(command.Id, businessRepository);
        }

        private static bool IsStandaloneSession(SessionAddCommand command, ServiceData service)
        {
            if (command.Repetition != null)
                return command.Repetition.SessionCount == 1;

            return service.Repetition.SessionCount == 1;
        }


        private LocationData GetLocationById(Guid locationId, IBusinessRepository businessRepository)
        {
            var location = businessRepository.Get(Id).Locations.FirstOrDefault(x => x.Id == locationId);
            if (location == null)
                throw new InvalidLocation();
            return location;
        }

        private CoachData GetCoachById(Guid coachId, IBusinessRepository businessRepository)
        {
            var coach = businessRepository.Get(Id).Coaches.FirstOrDefault(x => x.Id == coachId);
            if (coach == null)
                throw new InvalidCoach();
            return coach;
        }

        private ServiceData GetServiceById(Guid serviceId, IBusinessRepository businessRepository)
        {
            var service = businessRepository.Get(Id).Services.FirstOrDefault(x => x.Id == serviceId);
            if (service == null)
                throw new InvalidService();
            return service;
        }

        private SingleSessionData GetSessionById(Guid sessionId, IBusinessRepository businessRepository)
        {
            return businessRepository.Get(Id).Sessions.FirstOrDefault(x => x.Id == sessionId);
        }

        private RepeatedSessionData GetCourseById(Guid courseId, IBusinessRepository businessRepository)
        {
            return businessRepository.Get(Id).Courses.FirstOrDefault(x => x.Id == courseId);
        }

        private CustomerData GetCustomerById(Guid customerId, IBusinessRepository businessRepository)
        {
            var customer = businessRepository.Get(Id).Customers.FirstOrDefault(x => x.Id == customerId);
            if (customer == null)
                throw new InvalidCustomer();
            return customer;
        }
    }
}