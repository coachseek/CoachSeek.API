using System;
using System.Threading.Tasks;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public abstract class SessionBaseUseCase : BaseUseCase
    {
        protected async Task<Session> GetExistingSessionOrCourseAsync(Guid sessionId)
        {
            var session = await BusinessRepository.GetSessionAsync(Business.Id, sessionId);
            if (session.IsExisting())
                return await CreateSessionAsync(session);
            var course = await BusinessRepository.GetCourseAsync(Business.Id, sessionId);
            if (course.IsExisting())
                return await CreateCourseAsync(course);
            return null;
        }

        private async Task<SingleSession> CreateSessionAsync(SingleSessionData session)
        {
            if (session.ParentId.IsNotFound())
                return new StandaloneSession(session, await LookupCoreDataAsync(session));
            return new SessionInCourse(session, await LookupCoreDataAsync(session));
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
