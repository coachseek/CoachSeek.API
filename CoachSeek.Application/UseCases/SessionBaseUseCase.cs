using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public abstract class SessionBaseUseCase : BaseUseCase
    {
        protected Session GetExistingSessionOrCourse(Guid sessionId)
        {
            // Is it a Session or a Course?
            var session = BusinessRepository.GetSession(Business.Id, sessionId);
            if (session.IsExisting())
            {
                if (session.ParentId == null)
                    return new StandaloneSession(session, LookupCoreData(session));

                return new SessionInCourse(session, LookupCoreData(session));
            }

            var course = BusinessRepository.GetCourse(Business.Id, sessionId);
            if (course.IsExisting())
                return new RepeatedSession(course, LookupCoreData(course));

            return null;
        }


        private CoreData LookupCoreData(SessionData data)
        {
            var location = BusinessRepository.GetLocation(Business.Id, data.Location.Id);
            var coach = BusinessRepository.GetCoach(Business.Id, data.Coach.Id);
            var service = BusinessRepository.GetService(Business.Id, data.Service.Id);

            return new CoreData(location, coach, service);
        }
    }
}
