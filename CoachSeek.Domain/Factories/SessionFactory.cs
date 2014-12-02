using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Factories
{
    public static class SessionFactory
    {
        public static Session CreateNewSession(NewSessionData newSessionData, LocationData location, CoachData coach, ServiceData service)
        {
            var sessionData = new SessionData(Guid.NewGuid(), newSessionData);

            return CreateSession(sessionData, location, coach, service);
        }

        public static Session CreateSession(SessionData session, LocationData location, CoachData coach, ServiceData service)
        {
            if (IsStandaloneSession(session, service))
                return new StandaloneSession(session, location, coach, service);

            return new RepeatedSession(session, location, coach, service);
        }


        private static bool IsStandaloneSession(SessionData session, ServiceData service)
        {
            if (session.Repetition != null)
                return session.Repetition.SessionCount == 1;

            return service.Repetition.SessionCount == 1;
        }
    }
}
