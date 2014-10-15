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
        public IList<Location> Locations { get { return BusinessLocations.Locations; } }
        public IList<Coach> Coaches { get { return BusinessCoaches.Coaches; } }

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

        public Business(Guid id) 
            : this()
        {
            Id = id;
        }

        //public Business(IEnumerable<LocationData> locations, IEnumerable<CoachData> coaches) 
        //    : this()
        //{
        //    AppendExistingLocations(locations);
        //    AppendExistingCoaches(coaches);
        //}


        public void AddLocation(LocationAddCommand command, IBusinessRepository businessRepository)
        {
            BusinessLocations.Add(command.ToData());
            businessRepository.Save(this);
        }

        public void UpdateLocation(LocationUpdateCommand request, IBusinessRepository businessRepository)
        {
            BusinessLocations.Update(request.ToData());
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
            return new BusinessData
            {
                Id = Id,
                Name = Name,
                Domain = Domain,
                Admin = new BusinessAdminData
                {
                    Id = Admin.Id,
                    FirstName = Admin.FirstName,
                    LastName = Admin.LastName,
                    Email = Admin.Email,
                    Username = Admin.Username,
                    PasswordHash = Admin.PasswordHash
                }
            };
        }


        private void AppendExistingCoaches(IEnumerable<CoachData> coaches)
        {
            foreach (var coach in coaches)
                BusinessCoaches.Append(coach);
        }

        private void AppendExistingLocations(IEnumerable<LocationData> locations)
        {
            foreach (var location in locations)
                BusinessLocations.Append(location);
        }
    }
}