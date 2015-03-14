using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public class SessionDeleteUseCase : BaseUseCase, ISessionDeleteUseCase
    {
        public Response DeleteSession(Guid id)
        {
            var sessionOrCourse = GetExistingSessionOrCourse(id);
            if (sessionOrCourse == null)
                return new NotFoundResponse();

            if (sessionOrCourse is SingleSession)
                BusinessRepository.DeleteSession(BusinessId, id);
            else if (sessionOrCourse is RepeatedSession)
                BusinessRepository.DeleteCourse(BusinessId, id);

            return new Response();
        }


        private Session GetExistingSessionOrCourse(Guid sessionId)
        {
            // Is it a Session or a Course?
            var session = BusinessRepository.GetSession(BusinessId, sessionId);
            if (session.IsExisting())
            {
                if (session.ParentId == null)
                    return new StandaloneSession(session, LookupCoreData(session));

                return new SingleSession(session, LookupCoreData(session));
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