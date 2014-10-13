using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Data;
using CoachSeek.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace CoachSeek.Domain.Entities
{
    public class Business
    {
        private string _name;
        private string _domain;

        public Guid Id { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value.Trim(); }
        }

        public string Domain
        {
            get { return _domain; }
            set { _domain = value.Trim(); }
        }

        public BusinessAdmin Admin { get; set; }

        private BusinessLocations BusinessLocations { get; set; }
        private BusinessCoaches BusinessCoaches { get; set; }


        public IList<Location> Locations
        {
            get { return BusinessLocations.Locations; }
        }
        
        public IList<Coach> Coaches
        {
            get { return BusinessCoaches.Coaches; }
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

        public Business(IEnumerable<LocationData> locations, IEnumerable<CoachData> coaches) 
            : this()
        {
            AppendExistingLocations(locations);
            AppendExistingCoaches(coaches);
        }


        public void AddLocation(LocationAddRequest request, IBusinessRepository businessRepository)
        {
            BusinessLocations.Add(request.ToData());
            businessRepository.Save(this);
        }

        public void UpdateLocation(LocationUpdateRequest request, IBusinessRepository businessRepository)
        {
            BusinessLocations.Update(request.ToData());
            businessRepository.Save(this);
        }

        public void AddCoach(CoachAddRequest request, IBusinessRepository businessRepository)
        {
            BusinessCoaches.Add(request.ToData());
            businessRepository.Save(this);
        }

        public void UpdateCoach(CoachUpdateRequest request, IBusinessRepository businessRepository)
        {
            BusinessCoaches.Update(request.ToData());
            businessRepository.Save(this);
        }

        public BusinessData ToData()
        {
            return new BusinessData
            {
                Id = Id,
                Name = Name,
                Domain = Domain,
                BusinessAdmin = new BusinessAdminData
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