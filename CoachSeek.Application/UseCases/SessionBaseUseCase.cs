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
            var session = BusinessRepository.GetSession(BusinessId, sessionId);
            if (session.IsExisting())
            {
                if (session.ParentId == null)
                    return new StandaloneSession(session, LookupCoreData(session));

                return new SessionInCourse(session, LookupCoreData(session));
            }

            var course = BusinessRepository.GetCourse(BusinessId, sessionId);
            if (course.IsExisting())
                return new RepeatedSession(course, LookupCoreData(course));

            return null;
        }


        private CoreData LookupCoreData(SessionData data)
        {
            var location = BusinessRepository.GetLocation(BusinessId, data.Location.Id);
            var coach = BusinessRepository.GetCoach(BusinessId, data.Coach.Id);
            var service = BusinessRepository.GetService(BusinessId, data.Service.Id);

            return new CoreData(location, coach, service);
        }
    }
}
