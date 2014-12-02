using System.Linq;
using CoachSeek.Data.Model;
using System;
using System.Collections.Generic;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Factories;

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
            var newSession = SessionFactory.CreateNewSession(newSessionData, location, coach, service);

            ValidateAdd(newSession);
            Sessions.Add(newSession);

            return newSession.Id;
        }

        public void Append(SessionData sessionData, LocationData locationData, CoachData coachData, ServiceData serviceData)
        {
            // Data is already valid. Eg. It comes from the database.
            var session = SessionFactory.CreateSession(sessionData, locationData, coachData, serviceData);

            Sessions.Add(session);
        }

        public void Update(SessionData sessionData, LocationData locationData, CoachData coachData, ServiceData serviceData)
        {
            var session = SessionFactory.CreateSession(sessionData, locationData, coachData, serviceData);
            ValidateUpdate(session);
            ReplaceSessionInSessions(session);
        }

        public IList<SessionData> ToData()
        {
            return Sessions.Select(session => session.ToData()).ToList();
        }


        private void ReplaceSessionInSessions(Session session)
        {
            var updateSession = Sessions.Single(x => x.Id == session.Id);
            var updateIndex = Sessions.IndexOf(updateSession);
            Sessions[updateIndex] = session;
        }

        private void ValidateAdd(Session newSession)
        {
            if (Sessions.Any(session => session.IsOverlapping(newSession)))
                throw new ClashingSession();
        }

        private void ValidateUpdate(Session existingSession)
        {
            var isExistingSession = Sessions.Any(x => x.Id == existingSession.Id);
            if (!isExistingSession)
                throw new InvalidSession();

            if (Sessions.Any(session => session.IsOverlapping(existingSession)))
                throw new ClashingSession();
        }
    }
}
