using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public class SessionGetByIdUseCase : BaseUseCase, ISessionGetByIdUseCase
    {
        public SessionData GetSession(Guid id)
       { 
            var sessionOrCourse = GetExistingSessionOrCourse(id);
            if (sessionOrCourse == null)
                return null;

            if (sessionOrCourse is SingleSession)
            {
                var session = ((SingleSession)sessionOrCourse).ToData();
                session.Booking.Bookings = BusinessRepository.GetCustomerBookingsBySessionId(BusinessId, sessionOrCourse.Id);

                return session;
            }
            if (sessionOrCourse is RepeatedSession)
            {
                var course = ((RepeatedSession)sessionOrCourse).ToData();

                // TODO: Get Bookings for Course

                return course;
            }

            throw new InvalidOperationException("Unexpected session type!");
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

        private CoreData LookupCoreData(SessionData data)
        {
            var location = BusinessRepository.GetLocation(BusinessId, data.Location.Id);
            var coach = BusinessRepository.GetCoach(BusinessId, data.Coach.Id);
            var service = BusinessRepository.GetService(BusinessId, data.Service.Id);

            return new CoreData(location, coach, service);
        }
    }
}
