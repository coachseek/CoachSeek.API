using System.Linq;
using CoachSeek.Data.Model;
using System;
using System.Collections.Generic;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class BusinessSessions
    {
        private List<SingleSession> Sessions { get; set; }

        public BusinessCourses Courses { get; set; }    // Required for overlap session check.


        public BusinessSessions()
        {
            Sessions = new List<SingleSession>();
        }

        public BusinessSessions(IEnumerable<SingleSessionData> sessions,
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


        public Guid Add(SessionAddCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            var newSession = new StandaloneSession(command, location, coach, service);

            ValidateAdd(newSession);
            Sessions.Add(newSession);

            return newSession.Id;
        }

        public void Update(SessionUpdateCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            var existingSession = new StandaloneSession(command, location, coach, service);

            ValidateUpdate(existingSession);
            ReplaceSessionInSessions(existingSession);
        }

        public void Append(SingleSessionData session, LocationData location, CoachData coach, ServiceData service)
        {
            // Data is already valid. Eg. It comes from the database.
            Sessions.Add(new StandaloneSession(session, location, coach, service));
        }

        public bool IsOverlappingSessions(Session session)
        {
            return Sessions.Any(s => s.IsOverlapping(session));
        }

        public IList<SingleSessionData> ToData()
        {
            return Sessions.Select(session => session.ToData()).ToList();
        }


        private void ReplaceSessionInSessions(SingleSession session)
        {
            var updateSession = Sessions.Single(x => x.Id == session.Id);
            var updateIndex = Sessions.IndexOf(updateSession);
            Sessions[updateIndex] = session;
        }

        private void ValidateAdd(SingleSession newSession)
        {
            if (IsOverlapping(newSession))
                throw new ClashingSession();
        }

        private void ValidateUpdate(SingleSession existingSession)
        {
            var isExistingSession = Sessions.Any(x => x.Id == existingSession.Id);
            if (!isExistingSession)
                throw new InvalidSession();

            if (IsOverlapping(existingSession))
                throw new ClashingSession();
        }

        private bool IsOverlapping(SingleSession session)
        {
            return IsOverlappingSessions(session) || IsOverlappingCourses(session);
        }

        private bool IsOverlappingCourses(SingleSession singleSession)
        {
            return Courses.IsOverlappingCourses(singleSession);
        }
    }
}
