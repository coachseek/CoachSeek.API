using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public abstract class SessionBaseUseCase : BaseUseCase
    {
        protected bool IsStandaloneSession(SingleSessionData session)
        {
            return !session.ParentId.HasValue;
        }

        protected async Task<Session> GetExistingSessionOrCourseAsync(Guid sessionIdOrCourseId)
        {
            var session = await LookupSessionAsync(sessionIdOrCourseId);
            if (session.IsExisting())
                return await CreateSessionAsync(session);
            var course = await LookupCourseAsync(sessionIdOrCourseId);
            if (course.IsExisting())
                return await CreateCourseAsync(course);
            return null;
        }

        protected async Task<SingleSessionData> LookupSessionAsync(Guid sessionId)
        {
            var sessionTask = BusinessRepository.GetSessionAsync(Business.Id, sessionId);
            var bookingsTask =  BusinessRepository.GetCustomerBookingsBySessionIdAsync(Business.Id, sessionId);
            var session  = await sessionTask;
            if (session.IsNotFound())
                return null;
            session.Booking.Bookings = await bookingsTask;
            return session;
        }

        private async Task<SingleSession> CreateSessionAsync(SingleSessionData session)
        {
            if (session.ParentId.IsNotFound())
                return new StandaloneSession(session, await LookupCoreDataAsync(session));
            return new SessionInCourse(session, await LookupCoreDataAsync(session));
        }

        protected async Task<RepeatedSessionData> LookupCourseAsync(Guid courseId)
        {
            var courseTask = BusinessRepository.GetCourseAsync(Business.Id, courseId);
            var bookingsTask = BusinessRepository.GetCustomerBookingsByCourseIdAsync(Business.Id, courseId);
            var course = await courseTask;
            if (course.IsNotFound())
                return null;
            var bookings = await bookingsTask;
            AddBookingsToCourse(course, bookings);
            return course;
        }

        private void AddBookingsToCourse(RepeatedSessionData course, IList<CustomerBookingData> bookings)
        {
            course.Booking.Bookings = bookings.Where(x => x.SessionId == course.Id).ToList();
            foreach (var session in course.Sessions)
                session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
        }

        private async Task<RepeatedSession> CreateCourseAsync(RepeatedSessionData course)
        {
            var locationsTask = BusinessRepository.GetAllLocationsAsync(Business.Id);
            var coachesTask = BusinessRepository.GetAllCoachesAsync(Business.Id);
            var servicesTask = BusinessRepository.GetAllServicesAsync(Business.Id);
            await Task.WhenAll(locationsTask, coachesTask, servicesTask);

            return new RepeatedSession(course,
                                       locationsTask.Result,
                                       coachesTask.Result,
                                       servicesTask.Result);
        }

        private async Task<CoreData> LookupCoreDataAsync(SessionData data)
        {
            var locationTask = BusinessRepository.GetLocationAsync(Business.Id, data.Location.Id);
            var coachTask = BusinessRepository.GetCoachAsync(Business.Id, data.Coach.Id);
            var serviceTask = BusinessRepository.GetServiceAsync(Business.Id, data.Service.Id);
            await Task.WhenAll(locationTask, coachTask, serviceTask);

            return new CoreData(locationTask.Result, coachTask.Result, serviceTask.Result);
        }
    }
}
