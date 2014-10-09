using System;
using System.Collections.Generic;
using CoachSeek.Domain;
using CoachSeek.Domain.Data;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Models.UseCases.Requests;

namespace CoachSeek.WebUI.Models
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

        public Business(IEnumerable<Location> locations, IEnumerable<CoachData> coaches) 
            : this()
        {
            AppendExistingLocations(locations);
            AppendExistingCoaches(coaches);
        }


        public void AddLocation(Location location, IBusinessRepository businessRepository)
        {
            BusinessLocations.Add(location);
            businessRepository.Save(this);
        }

        public void UpdateLocation(Location location, IBusinessRepository businessRepository)
        {
            BusinessLocations.Update(location);
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


        private void AppendExistingCoaches(IEnumerable<CoachData> coaches)
        {
            foreach (var coach in coaches)
                BusinessCoaches.Append(coach);
        }

        private void AppendExistingLocations(IEnumerable<Location> locations)
        {
            foreach (var location in locations)
                BusinessLocations.Add(location);
        }
    }
}