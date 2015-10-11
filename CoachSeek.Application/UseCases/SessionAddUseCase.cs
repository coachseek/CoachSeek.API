using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class SessionAddUseCase : BaseUseCase, ISessionAddUseCase
    {
        public async Task<IResponse> AddSessionAsync(SessionAddCommand command)
        {
            try
            {
                var coreData = await LookupCoreDataAsync(command);

                if (IsStandaloneSession(command, coreData.Service))
                    return await CreateStandaloneSessionAsync(command, coreData);

                return await CreateCourseAsync(command, coreData);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private async Task<CoreData> LookupCoreDataAsync(SessionAddCommand command)
        {
            var locationTask = BusinessRepository.GetLocationAsync(Business.Id, command.Location.Id);
            var coachTask = BusinessRepository.GetCoachAsync(Business.Id, command.Coach.Id);
            var serviceTask = BusinessRepository.GetServiceAsync(Business.Id, command.Service.Id);
            await Task.WhenAll(locationTask, coachTask, serviceTask);

            return new CoreData(locationTask.Result, coachTask.Result, serviceTask.Result);
        }

        private static bool IsStandaloneSession(SessionAddCommand command, ServiceData service)
        {
            if (command.Repetition != null)
                return command.Repetition.SessionCount == 1;

            return service.Repetition.SessionCount == 1;
        }

        private async Task<Response> CreateStandaloneSessionAsync(SessionAddCommand command, CoreData coreData)
        {
            var newSession = new StandaloneSession(command, coreData);
            await ValidateAddAsync(newSession);
            await BusinessRepository.AddSessionAsync(Business.Id, newSession);
            return new Response(newSession.ToData());
        }

        private async Task<Response> CreateCourseAsync(SessionAddCommand command, CoreData coreData)
        {
            var newCourse = new RepeatedSession(command, coreData);
            await ValidateAddAsync(newCourse);
            await BusinessRepository.AddCourseAsync(Business.Id, newCourse);
            return new Response(newCourse.ToData());
        }

        private async Task ValidateAddAsync(StandaloneSession newSession)
        {
            await ValidateIsNotOverlappingAsync(newSession);
        }

        private async Task ValidateIsNotOverlappingAsync(SingleSession newSession)
        {
            await ValidateIsNotOverlappingSessionsAsync(newSession);
        }

        private async Task ValidateIsNotOverlappingSessionsAsync(SingleSession newSession)
        {
            foreach (var session in await GetAllSessionsAsync())
            {
                if (newSession.IsOverlapping(session))
                    throw new SessionClashing(session);
            }
        }

        private async Task ValidateAddAsync(RepeatedSession newCourse)
        {
            await ValidateIsNotOverlappingAsync(newCourse);
        }

        private async Task ValidateIsNotOverlappingAsync(RepeatedSession course)
        {
            await ValidateIsNotOverlappingSessionsAsync(course);
        }

        private async Task ValidateIsNotOverlappingSessionsAsync(RepeatedSession newCourse)
        {
            foreach (var session in await GetAllSessionsAsync())
            {
                if (newCourse.IsOverlapping(session))
                    throw new SessionClashing(session);
            }
        }

        private async Task<List<SingleSession>> GetAllSessionsAsync()
        {
            var locationsTask = BusinessRepository.GetAllLocationsAsync(Business.Id);
            var coachesTask = BusinessRepository.GetAllCoachesAsync(Business.Id);
            var servicesTask = BusinessRepository.GetAllServicesAsync(Business.Id);
            var sessionsTask = BusinessRepository.GetAllSessionsAsync(Business.Id);
            await Task.WhenAll(locationsTask, coachesTask, servicesTask, sessionsTask);

            var sessions = new List<SingleSession>();
            foreach (var session in sessionsTask.Result)
            {
                var location = locationsTask.Result.Single(x => x.Id == session.Location.Id);
                var coach = coachesTask.Result.Single(x => x.Id == session.Coach.Id);
                var service = servicesTask.Result.Single(x => x.Id == session.Service.Id);

                sessions.Add(new SingleSession(session, location, coach, service));
            }

            return sessions;
        }
    }
}
