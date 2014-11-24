using System.Linq;
using CoachSeek.Data.Model;
using System;
using System.Collections.Generic;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class BusinessSessions
    {
        private List<Session> Sessions { get; set; }

        public BusinessSessions()
        {
            Sessions = new List<Session>();
        }

        public BusinessSessions(IEnumerable<SessionData> sessions,
                                BusinessLocations locations, 
                                BusinessCoaches coaches,
                                BusinessServices services)
            : this()
        {
            if (sessions == null || locations == null || coaches == null || services == null)
                return;

            foreach (var session in sessions)
            {
                var location = locations.GetById(session.Location.Id);
                var coach = coaches.GetById(session.Coach.Id);
                var service = services.GetById(session.Service.Id);
                Append(session, location, coach, service);                
            }
        }

        public Guid Add(NewSessionData newSessionData, LocationData location, CoachData coach, ServiceData service)
        {
            var newSession = new NewSession(newSessionData, location, coach, service);
            ValidateAdd(newSession);
            Sessions.Add(newSession);

            return newSession.Id;
        }

        public void Append(SessionData sessionData, LocationData locationData, CoachData coachData, ServiceData serviceData)
        {
            // Data is already valid. Eg. It comes from the database.
            Sessions.Add(new Session(sessionData, locationData, coachData, serviceData));
        }

        //public void Update(ServiceData serviceData)
        //{
        //    var service = new Service(serviceData);
        //    ValidateUpdate(service);
        //    ReplaceServiceInServices(service);
        //}

        public IList<SessionData> ToData()
        {
            return Sessions.Select(session => session.ToData()).ToList();
        }


        //private void ReplaceServiceInServices(Service service)
        //{
        //    var updateService = Services.Single(x => x.Id == service.Id);
        //    var updateIndex = Services.IndexOf(updateService);
        //    Services[updateIndex] = service;
        //}

        private void ValidateAdd(NewSession newSession)
        {
            if (Sessions.Any(session => session.IsOverlapping(newSession)))
                throw new ClashingSession();
        }

        //private void ValidateUpdate(Service service)
        //{
        //    var isExistingService = Services.Any(x => x.Id == service.Id);
        //    if (!isExistingService)
        //        throw new InvalidService();

        //    var existingService = Services.FirstOrDefault(x => x.Name.ToLower() == service.Name.ToLower());
        //    if (existingService != null && existingService.Id != service.Id)
        //        throw new DuplicateService();
        //}
    }
}
