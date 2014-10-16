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

        protected BusinessAdmin BusinessAdmin { get; set; }
        private BusinessLocations BusinessLocations { get; set; }
        private BusinessCoaches BusinessCoaches { get; set; }


        public Business(Guid id, 
            string name, 
            string domain, 
            BusinessAdminData admin,
            IEnumerable<LocationData> locations, 
            IEnumerable<CoachData> coaches)
        {
            Id = id;
            Name = name;
            Domain = domain;
            BusinessAdmin = new BusinessAdmin(admin);
            BusinessLocations = new BusinessLocations(locations);
            BusinessCoaches = new BusinessCoaches(coaches);
        }

        public Business()
        {
            Id = Guid.NewGuid();
            BusinessLocations = new BusinessLocations();
            BusinessCoaches = new BusinessCoaches();
        }

        // Minimal Unit testing constructor.
        public Business(Guid id)
            : this()
        {
            Id = id;
        }


        public void AddLocation(LocationAddCommand command, IBusinessRepository businessRepository)
        {
            BusinessLocations.Add(command.ToData());
            businessRepository.Save(this);
        }

        public void UpdateLocation(LocationUpdateCommand command, IBusinessRepository businessRepository)
        {
            BusinessLocations.Update(command.ToData());
            businessRepository.Save(this);
        }

        public void AddCoach(CoachAddCommand command, IBusinessRepository businessRepository)
        {
            BusinessCoaches.Add(command.ToData());
            businessRepository.Save(this);
        }

        public void UpdateCoach(CoachUpdateCommand command, IBusinessRepository businessRepository)
        {
            BusinessCoaches.Update(command.ToData());
            businessRepository.Save(this);
        }

        public BusinessData ToData()
        {
            return Mapper.Map<Business, BusinessData>(this);
        }
    }
}