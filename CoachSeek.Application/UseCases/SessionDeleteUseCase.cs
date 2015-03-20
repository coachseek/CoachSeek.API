using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class SessionDeleteUseCase : BaseUseCase, ISessionDeleteUseCase
    {
        public Response DeleteSession(Guid id)
        {
            var sessionOrCourse = GetExistingSessionOrCourse(id);
            if (sessionOrCourse == null)
                return new NotFoundResponse();

            try
            {
                if (sessionOrCourse is SingleSession)
                    TryDeleteSession(id);
                else if (sessionOrCourse is RepeatedSession)
                    TryDeleteCourse(id);

                return new Response();
            }
            catch (Exception ex)
            {
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }


        private Session GetExistingSessionOrCourse(Guid sessionId)
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

        private void TryDeleteSession(Guid sessionId)
        {
            var bookings = BusinessRepository.GetCustomerBookingsBySessionId(BusinessId, sessionId);
            if (bookings.Count > 0)
                throw new ValidationException("Cannot delete session as it has one or more bookings.");

            BusinessRepository.DeleteSession(BusinessId, sessionId);
        }

        private void TryDeleteCourse(Guid courseId)
        {
            // TODO: Determine whether there are any session or course bookings.

            BusinessRepository.DeleteCourse(BusinessId, courseId);
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