using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Domain.Services
{
    public static class SessionsInCourseValidator
    {
        public static void Validate(IEnumerable<SessionKeyCommand> sessionsToCheck, RepeatedSessionData course)
        {
            var sessions = sessionsToCheck.ToList();

            var errors = new ValidationException();

            ValidateSessionsAreInCourse(sessions, course, errors);
            ValidateSessionsAreUnique(sessions, errors);
           
            errors.ThrowIfErrors();
        }


        private static void ValidateSessionsAreInCourse(IEnumerable<SessionKeyCommand> sessionsToCheck,
                                                        RepeatedSessionData course,
                                                        ValidationException errors)
        {
            foreach (var session in sessionsToCheck)
                if (!course.Sessions.Select(x => x.Id).Contains(session.Id))
                    errors.Add(new SessionNotInCourse(session.Id, course.Id));
        }

        private static void ValidateSessionsAreUnique(IEnumerable<SessionKeyCommand> sessionsToCheck, ValidationException errors)
        {
            if (sessionsToCheck.GroupBy(x => x.Id).SelectMany(grp => grp.Skip(1)).Any())
                errors.Add(new SessionDuplicate());
        }
    }
}
