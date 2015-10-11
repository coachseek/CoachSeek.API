using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class SessionUpdateUseCase : SessionBaseUseCase, ISessionUpdateUseCase
    {
        public async Task<IResponse> UpdateSessionAsync(SessionUpdateCommand command)
        {
            try
            {
                var sessionOrCourse = await GetExistingSessionOrCourseAsync(command.Id);
                if (sessionOrCourse.IsNotFound())
                    return new NotFoundResponse();
                return await UpdateSessionOrCourseAsync(command, sessionOrCourse);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private async Task<IResponse> UpdateSessionOrCourseAsync(SessionUpdateCommand command, Session existingSessionOrCourse)
        {
            if (existingSessionOrCourse is StandaloneSession)
                return await UpdateStandaloneSessionAsync(command, existingSessionOrCourse as StandaloneSession);
            if (existingSessionOrCourse is SessionInCourse)
                return await UpdateSessionInCourseAsync(command, existingSessionOrCourse as SessionInCourse);
            if (existingSessionOrCourse is RepeatedSession)
                return await UpdateCourseAsync(command, (RepeatedSession)existingSessionOrCourse);

            throw new InvalidOperationException("Unexpected session type!");
        }

        private async Task<IResponse> UpdateStandaloneSessionAsync(SessionUpdateCommand command, StandaloneSession existingSession)
        {
            if (IsChangingSessionToCourse(command))
                throw new SessionChangeToCourseNotSupported();
            var coreData = await LookupAndValidateCoreDataAsync(command);
            var updateSession = new StandaloneSession(existingSession, command, coreData);
            ValidateUpdate(updateSession);
            BusinessRepository.UpdateSession(Business.Id, updateSession);
            return new Response(updateSession.ToData());
        }

        private async Task<IResponse> UpdateSessionInCourseAsync(SessionUpdateCommand command, SessionInCourse existingSession)
        {
            if (IsChangingSessionToCourse(command))
                throw new SessionChangeToCourseNotSupported();
            var coreData = await LookupAndValidateCoreDataAsync(command);
            var updateSession = new SessionInCourse(existingSession, command, coreData);
            ValidateUpdate(updateSession);
            BusinessRepository.UpdateSession(Business.Id, updateSession);
            return new Response(updateSession.ToData());
        }

        private async Task<IResponse> UpdateCourseAsync(SessionUpdateCommand command, RepeatedSession existingCourse)
        {
            var coreData = await LookupAndValidateCoreDataAsync(command);
            var updateCourse = new RepeatedSession(existingCourse, command, coreData);
            ValidateUpdate(existingCourse, updateCourse);
            BusinessRepository.UpdateCourse(Business.Id, updateCourse);
            return new Response(updateCourse.ToData());
        }

        private bool IsChangingSessionToCourse(SessionUpdateCommand command)
        {
            return command.Repetition != null && (command.Repetition.SessionCount != 1 || command.Repetition.RepeatFrequency != null);
        }

        private async Task<CoreData> LookupAndValidateCoreDataAsync(SessionUpdateCommand command)
        {
            var locationTask = BusinessRepository.GetLocationAsync(Business.Id, command.Location.Id);
            var coachTask = BusinessRepository.GetCoachAsync(Business.Id, command.Coach.Id);
            var serviceTask = BusinessRepository.GetServiceAsync(Business.Id, command.Service.Id);
            await Task.WhenAll(locationTask, coachTask, serviceTask);

            if (!locationTask.Result.IsExisting())
                throw new LocationInvalid(command.Location.Id);
            if (!coachTask.Result.IsExisting())
                throw new CoachInvalid(command.Coach.Id);
            if (!serviceTask.Result.IsExisting())
                throw new ServiceInvalid(command.Service.Id);

            var coreData = new CoreData(locationTask.Result, coachTask.Result, serviceTask.Result);

            ValidateCoreData(coreData);

            return coreData;
        }

        private void ValidateUpdate(SingleSession updateSession)
        {
            ValidateIsNotOverlapping(updateSession);
        }

        private void ValidateUpdate(RepeatedSession existingCourse, RepeatedSession updateCourse)
        {
            if (HasDifferingCourseRepetitions(existingCourse, updateCourse))
                throw new CourseChangeRepetitionNotSupported();
        }

        private bool HasDifferingCourseRepetitions(RepeatedSession existingCourse, RepeatedSession updateCourse)
        {
            return (existingCourse.Repetition.SessionCount != updateCourse.Repetition.SessionCount
                    || existingCourse.Repetition.RepeatFrequency != updateCourse.Repetition.RepeatFrequency);
        }

        private void ValidateCoreData(CoreData coreData)
        {
            var errors = new ValidationException();

            if (!coreData.Location.IsExisting())
                errors.Add(new LocationInvalid(coreData.Location.Id));
            if (!coreData.Coach.IsExisting())
                errors.Add(new CoachInvalid(coreData.Coach.Id));
            if(!coreData.Service.IsExisting())
                errors.Add(new ServiceInvalid(coreData.Service.Id));

            errors.ThrowIfErrors();
        }

        private void ValidateIsNotOverlapping(SingleSession command)
        {
            ValidateIsNotOverlappingSessions(command);
        }

        private void ValidateIsNotOverlappingSessions(SingleSession session)
        {
            var singleSessions = GetAllSessions();

            foreach (var singleSession in singleSessions)
            {
                if (session.IsOverlapping(singleSession))
                    throw new SessionClashing(singleSession);
            }
        }

        private List<SingleSession> GetAllSessions()
        {
            var locations = BusinessRepository.GetAllLocations(Business.Id);
            var coaches = BusinessRepository.GetAllCoaches(Business.Id);
            var services = BusinessRepository.GetAllServices(Business.Id);

            var sessionDatas = BusinessRepository.GetAllSessions(Business.Id);

            var sessions = new List<SingleSession>();
            foreach (var sessionData in sessionDatas)
            {
                var location = locations.Single(x => x.Id == sessionData.Location.Id);
                var coach = coaches.Single(x => x.Id == sessionData.Coach.Id);
                var service = services.Single(x => x.Id == sessionData.Service.Id);

                var session = new SingleSession(sessionData, location, coach, service);
                sessions.Add(session);
            }

            return sessions;
        }
    }
}
