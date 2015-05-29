using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
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
        public Response UpdateSession(SessionUpdateCommand command)
        {
            var sessionOrCourse = GetExistingSessionOrCourse(command.Id);
            if (sessionOrCourse == null)
                return new NotFoundResponse();

            return UpdateSessionOrCourse(command, sessionOrCourse);
        }


        private Response UpdateSessionOrCourse(SessionUpdateCommand command, Session existingSessionOrCourse)
        {
            if (existingSessionOrCourse is StandaloneSession)
                return HandleUpdateStandaloneSession(command, (StandaloneSession)existingSessionOrCourse);
            if (existingSessionOrCourse is SessionInCourse)
                return HandleUpdateSessionInCourse(command, (SessionInCourse)existingSessionOrCourse);
            if (existingSessionOrCourse is RepeatedSession)
                return HandleUpdateCourse(command, (RepeatedSession)existingSessionOrCourse);

            throw new InvalidOperationException("Unexpected session type!");
        }

        private Response HandleUpdateStandaloneSession(SessionUpdateCommand command, StandaloneSession existingSession)
        {
            if (IsChangingSessionToCourse(command))
                return new CannotChangeSessionToCourseErrorResponse();

            return UpdateStandaloneSession(existingSession, command);
        }

        private Response HandleUpdateSessionInCourse(SessionUpdateCommand command, SessionInCourse existingSession)
        {
            if (IsChangingSessionToCourse(command))
                return new CannotChangeSessionToCourseErrorResponse();

            return UpdateSessionInCourse(existingSession, command);
        }

        private Response HandleUpdateCourse(SessionUpdateCommand command, RepeatedSession existingCourse)
        {
            try
            {
                var coreData = LookupAndValidateCoreData(command);
                var updateCourse = new RepeatedSession(existingCourse, command, coreData);
                ValidateUpdate(existingCourse, updateCourse);
                var data = BusinessRepository.UpdateCourse(Business.Id, updateCourse);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is ClashingSession)
                    return new ClashingSessionErrorResponse((ClashingSession)ex);
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
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

            var coreData = new CoreData(location, coach, service);

            ValidateCoreData(coreData);

            return coreData;
        }

        private Response UpdateStandaloneSession(StandaloneSession existingSession, SessionUpdateCommand command)
        {
            try
            {
                var coreData = LookupAndValidateCoreData(command);
                var updateSession = new StandaloneSession(existingSession, command, coreData);
                ValidateUpdate(updateSession);
                var data = BusinessRepository.UpdateSession(Business.Id, updateSession);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is ClashingSession)
                    return new ClashingSessionErrorResponse((ClashingSession)ex);
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private Response UpdateSessionInCourse(SessionInCourse existingSession, SessionUpdateCommand command)
        {
            try
            {
                var coreData = LookupAndValidateCoreData(command);
                var updateSession = new SessionInCourse(existingSession, command, coreData);
                ValidateUpdate(updateSession);
                var data = BusinessRepository.UpdateSession(Business.Id, updateSession);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is ClashingSession)
                    return new ClashingSessionErrorResponse((ClashingSession)ex);
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateUpdate(SingleSession updateSession)
        {
            ValidateIsNotOverlapping(updateSession);
        }

        private void ValidateUpdate(RepeatedSession existingCourse, RepeatedSession updateCourse)
        {
            if (HasDifferingCourseRepetitions(existingCourse, updateCourse))
                throw new ValidationException("Cannot change the repetition of a course.");
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
                    throw new ClashingSession(singleSession);
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
