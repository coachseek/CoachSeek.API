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
    public class SessionUpdateUseCase : BaseUseCase, ISessionUpdateUseCase
    {
        public Response UpdateSession(SessionUpdateCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            var session = GetExistingSessionOrCourse(command.Id);
            if (session == null)
                return new NotFoundResponse();

            if (session is StandaloneSession)
            {
                if (IsChangingSessionToCourse(command))
                    return new CannotChangeSessionToCourseErrorResponse();
                return UpdateStandaloneSession(command);
            }
            if (session is SingleSession)
            {
                if (IsChangingSessionToCourse(command))
                    return new CannotChangeSessionToCourseErrorResponse();
                return UpdateSessionInCourse(command);
            }
            if (session is RepeatedSession)
            {
                // TODO: Make courses updatable.
                return new CannotUpdateCourseErrorResponse();
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

                return new SingleSession(session, LookupCoreData(session));
            }

            var course = BusinessRepository.GetCourse(BusinessId, sessionId);
            if (course.IsExisting())
                return new RepeatedSession(course, LookupCoreData(course));

            return null;
        }

        private bool IsChangingSessionToCourse(SessionUpdateCommand command)
        {
            return command.Repetition != null && (command.Repetition.SessionCount != 1 || command.Repetition.RepeatFrequency != null);
        }

        private CoreData LookupCoreData(SessionData data)
        {
            var location = BusinessRepository.GetLocation(BusinessId, data.Location.Id);
            var coach = BusinessRepository.GetCoach(BusinessId, data.Coach.Id);
            var service = BusinessRepository.GetService(BusinessId, data.Service.Id);

            return new CoreData(location, coach, service);
        }

        private CoreData LookupAndValidateCoreData(SessionUpdateCommand command)
        {
            var location = BusinessRepository.GetLocation(BusinessId, command.Location.Id);
            var coach = BusinessRepository.GetCoach(BusinessId, command.Coach.Id);
            var service = BusinessRepository.GetService(BusinessId, command.Service.Id);

            var coreData = new CoreData(location, coach, service);

            ValidateCoreData(coreData);

            return coreData;
        }

        private Response UpdateStandaloneSession(SessionUpdateCommand command)
        {
            try
            {
                var coreData = LookupAndValidateCoreData(command);
                var updateSession = new StandaloneSession(command, coreData);
                ValidateUpdate(updateSession);
                var data = BusinessRepository.UpdateSession(BusinessId, updateSession);
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

        private Response UpdateSessionInCourse(SessionUpdateCommand command)
        {
            try
            {
                var coreData = LookupAndValidateCoreData(command);
                var updateSession = new SingleSession(command, coreData);
                ValidateUpdate(updateSession);
                var data = BusinessRepository.UpdateSession(BusinessId, updateSession);
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
            var locations = BusinessRepository.GetAllLocations(BusinessId);
            var coaches = BusinessRepository.GetAllCoaches(BusinessId);
            var services = BusinessRepository.GetAllServices(BusinessId);

            var sessionDatas = BusinessRepository.GetAllSessions(BusinessId);

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
