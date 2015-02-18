using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Factories
{
    public static class SessionFactory
    {
        public static Session CreateNewSession(SessionAddCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            if (IsStandaloneSession(command, service))
                return new StandaloneSession(command, location, coach, service);

            return new RepeatedSession(command, location, coach, service);
        }

        public static Session CreateSession(SessionUpdateCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            if (IsStandaloneSession(command, service))
                return new StandaloneSession(command, location, coach, service);

            return new RepeatedSession(command, location, coach, service);
        }


        //private static bool IsStandaloneSession(SessionData session, ServiceData service)
        //{
        //    if (session.Repetition != null)
        //        return session.Repetition.SessionCount == 1;

        //    return service.Repetition.SessionCount == 1;
        //}

        private static bool IsStandaloneSession(SessionAddCommand command, ServiceData service)
        {
            if (command.Repetition != null)
                return command.Repetition.SessionCount == 1;

            return service.Repetition.SessionCount == 1;
        }
    }
}
