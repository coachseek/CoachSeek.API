using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class SessionUpdateUseCase : UpdateUseCase, ISessionUpdateUseCase
    {
        public Guid BusinessId { get; set; }


        public SessionUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response UpdateSession(SessionUpdateCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            var session = GetExistingSessionOrCourse(command.Id);
            if (session == null)
                return new NotFoundResponse();
            if (session is StandaloneSession)
            {
                if (command.Repetition != null && command.Repetition.SessionCount > 1)
                    return new CannotChangeStandaloneSessionToCourseErrorResponse();
                return UpdateStandaloneSession((StandaloneSession)session, command);
            }
            if (session is RepeatedSession)
            {
                // TODO: Make standalone sessions and then courses updatable.
                return new CannotUpdateCourseErrorResponse();
            }


            //var session = GetSessionById(command.Id, businessRepository);
            //if (session.IsExisting())
            //    ValidateOrDefaultSessionRepetition(command);
            //else
            //    ValidateOrDefaultCourseRepetition(command, businessRepository);

            //if (IsStandaloneSession(command, coreData.Service))
            //    return UpdateStandaloneSession(command, businessRepository, coreData);

            //return UpdateCourse(command, businessRepository, coreData);

            return null;
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

        private CoreData LookupAndValidateCoreData(SessionUpdateCommand command)
        {
            var location = BusinessRepository.GetLocation(BusinessId, command.Location.Id);
            var coach = BusinessRepository.GetCoach(BusinessId, command.Coach.Id);
            var service = BusinessRepository.GetService(BusinessId, command.Service.Id);

            var coreData = new CoreData(location, coach, service);

            ValidateCoreData(coreData);

            return coreData;
        }

        private Response UpdateStandaloneSession(StandaloneSession existingSession, SessionUpdateCommand command)
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

        private void ValidateUpdate(StandaloneSession updateSession)
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

        private void ValidateIsNotOverlapping(StandaloneSession command)
        {
            ValidateIsNotOverlappingSessions(command);
        }

        private void ValidateIsNotOverlappingSessions(StandaloneSession session)
        {
            var standaloneSessions = GetAllSessions();

            foreach (var standaloneSession in standaloneSessions)
            {
                if (session.IsOverlapping(standaloneSession))
                    throw new ClashingSession(standaloneSession);
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

        protected override object UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateSession((SessionUpdateCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is InvalidSession)
                return new InvalidSessionErrorResponse();
            //if (ex is ClashingSession)
            //    return new ClashingSessionErrorResponse();

            return null;
        }
    }
}
