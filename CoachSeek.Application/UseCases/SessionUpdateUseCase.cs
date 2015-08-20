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
        public IResponse UpdateSession(SessionUpdateCommand command)
        {
            try
            {
                var sessionOrCourse = GetExistingSessionOrCourse(command.Id);
                if (sessionOrCourse.IsNotFound())
                    return new NotFoundResponse();
                return UpdateSessionOrCourse(command, sessionOrCourse);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private IResponse UpdateSessionOrCourse(SessionUpdateCommand command, Session existingSessionOrCourse)
        {
            if (existingSessionOrCourse is StandaloneSession)
                return UpdateStandaloneSession(command, existingSessionOrCourse as StandaloneSession);
            if (existingSessionOrCourse is SessionInCourse)
                return UpdateSessionInCourse(command, existingSessionOrCourse as SessionInCourse);
            if (existingSessionOrCourse is RepeatedSession)
                return UpdateCourse(command, (RepeatedSession)existingSessionOrCourse);

            throw new InvalidOperationException("Unexpected session type!");
        }

        private IResponse UpdateStandaloneSession(SessionUpdateCommand command, StandaloneSession existingSession)
        {
            if (IsChangingSessionToCourse(command))
                throw new SessionChangeToCourseNotSupported();
            var coreData = LookupAndValidateCoreData(command);
            var updateSession = new StandaloneSession(existingSession, command, coreData);
            ValidateUpdate(updateSession);
            var data = BusinessRepository.UpdateSession(Business.Id, updateSession);
            return new Response(data);
        }

        private IResponse UpdateSessionInCourse(SessionUpdateCommand command, SessionInCourse existingSession)
        {
            if (IsChangingSessionToCourse(command))
                throw new SessionChangeToCourseNotSupported();
            var coreData = LookupAndValidateCoreData(command);
            var updateSession = new SessionInCourse(existingSession, command, coreData);
            ValidateUpdate(updateSession);
            var data = BusinessRepository.UpdateSession(Business.Id, updateSession);
            return new Response(data);
        }

        private IResponse UpdateCourse(SessionUpdateCommand command, RepeatedSession existingCourse)
        {
            var coreData = LookupAndValidateCoreData(command);
            var updateCourse = new RepeatedSession(existingCourse, command, coreData);
            ValidateUpdate(existingCourse, updateCourse);
            var data = BusinessRepository.UpdateCourse(Business.Id, updateCourse);
            return new Response(data);
        }

        private bool IsChangingSessionToCourse(SessionUpdateCommand command)
        {
            return command.Repetition != null && (command.Repetition.SessionCount != 1 || command.Repetition.RepeatFrequency != null);
        }

        private CoreData LookupAndValidateCoreData(SessionUpdateCommand command)
        {
            var location = BusinessRepository.GetLocation(Business.Id, command.Location.Id);
            var coach = BusinessRepository.GetCoach(Business.Id, command.Coach.Id);
            var service = BusinessRepository.GetService(Business.Id, command.Service.Id);

            if (!location.IsExisting())
                throw new LocationInvalid(command.Location.Id);
            if (!coach.IsExisting())
                throw new CoachInvalid(command.Coach.Id);
            if (!service.IsExisting())
                throw new ServiceInvalid(command.Service.Id);

            var coreData = new CoreData(location, coach, service);

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
            var validationException = new ValidationException();

            if (coreData.Location == null)
                validationException.Add("Invalid location.");

            if (coreData.Coach == null)
                validationException.Add("Invalid coach.");

            if(coreData.Service == null)
                validationException.Add("Invalid service.");

            validationException.ThrowIfErrors();
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
