using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
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
        public BusinessAdminData Admin { get { return BusinessAdmin.ToData(); } }
        public IList<LocationData> Locations { get { return BusinessLocations.ToData(); } }
        public IList<CoachData> Coaches { get { return BusinessCoaches.ToData(); } }
        public IList<ServiceData> Services { get { return BusinessServices.ToData(); } }

        protected BusinessAdmin BusinessAdmin { get; set; }
        private BusinessLocations BusinessLocations { get; set; }
        private BusinessCoaches BusinessCoaches { get; set; }
        private BusinessServices BusinessServices { get; set; }
        private BusinessSessions BusinessSessions { get; set; }


        public Business(Guid id, 
            string name, 
            string domain, 
            BusinessAdminData admin,
            IEnumerable<LocationData> locations, 
            IEnumerable<CoachData> coaches,
            IEnumerable<ServiceData> services
            )
        {
            Id = id;
            Name = name;
            Domain = domain;
            BusinessAdmin = new BusinessAdmin(admin);
            BusinessLocations = new BusinessLocations(locations);
            BusinessCoaches = new BusinessCoaches(coaches);
            BusinessServices = new BusinessServices(services);
        }

        public Business()
        {
            Id = Guid.NewGuid();
            BusinessLocations = new BusinessLocations();
            BusinessCoaches = new BusinessCoaches();
            BusinessServices = new BusinessServices();
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

        public SessionData AddSession(SessionAddCommand command, IBusinessRepository businessRepository)
        {
            var sessionId = BusinessSessions.Add(command.ToData());
            businessRepository.Save(this);

            return GetSessionById(sessionId, businessRepository);
        }


        public BusinessData ToData()
        {
            return Mapper.Map<Business, BusinessData>(this);
        }


        private LocationData GetLocationById(Guid locationId, IBusinessRepository businessRepository)
        {
            return businessRepository.Get(Id).Locations.Single(x => x.Id == locationId);
        }

        private CoachData GetCoachById(Guid coachId, IBusinessRepository businessRepository)
        {
            return businessRepository.Get(Id).Coaches.Single(x => x.Id == coachId);
        }

        private ServiceData GetServiceById(Guid serviceId, IBusinessRepository businessRepository)
        {
            return businessRepository.Get(Id).Services.Single(x => x.Id == serviceId);
        }

        private SessionData GetSessionById(Guid sessionId, IBusinessRepository businessRepository)
        {
            return null;

            //return businessRepository.Get(Id).Sessions.Single(x => x.Id == sessionId);
        }
    }
}