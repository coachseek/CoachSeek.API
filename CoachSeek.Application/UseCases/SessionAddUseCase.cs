using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class SessionAddUseCase : AddUseCase, ISessionAddUseCase
    {
        public Guid BusinessId { get; set; }

        
        public SessionAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddSession(SessionAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var coreData = LookupCoreData(command);

                if (IsStandaloneSession(command, coreData.Service))
                    return CreateStandaloneSession(command, coreData);

                return CreateCourse(command, coreData);
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


        private CoreData LookupCoreData(SessionAddCommand command)
        {
            var location = BusinessRepository.GetLocation(BusinessId, command.Location.Id);
            var coach = BusinessRepository.GetCoach(BusinessId, command.Coach.Id);
            var service = BusinessRepository.GetService(BusinessId, command.Service.Id);

            return new CoreData(location, coach, service);
        }

        private static bool IsStandaloneSession(SessionAddCommand command, ServiceData service)
        {
            if (command.Repetition != null)
                return command.Repetition.SessionCount == 1;

            return service.Repetition.SessionCount == 1;
        }

        private Response CreateStandaloneSession(SessionAddCommand command, CoreData coreData)
        {
            var newSession = new StandaloneSession(command, coreData);
            ValidateAdd(newSession);
            var data = BusinessRepository.AddSession(BusinessId, newSession);
            return new Response(data);
        }

        private Response CreateCourse(SessionAddCommand command, CoreData coreData)
        {
            var newCourse = new RepeatedSession(command, coreData);
            ValidateAdd(newCourse);
            var data = BusinessRepository.AddCourse(BusinessId, newCourse);
            return new Response(data);
        }

        private void ValidateAdd(StandaloneSession newSession)
        {
            ValidateIsNotOverlapping(newSession);
        }

        private void ValidateIsNotOverlapping(SingleSession newSession)
        {
            ValidateIsNotOverlappingSessions(newSession);
        }

        private void ValidateIsNotOverlappingSessions(SingleSession newSession)
        {
            var standaloneSessions = GetAllSessions();

            foreach (var standaloneSession in standaloneSessions)
            {
                if (newSession.IsOverlapping(standaloneSession))
                    throw new ClashingSession(standaloneSession);
            }
        }

        private void ValidateAdd(RepeatedSession newCourse)
        {
            ValidateIsNotOverlapping(newCourse);
        }

        private void ValidateIsNotOverlapping(RepeatedSession course)
        {
            ValidateIsNotOverlappingStandaloneSessions(course);
            // ValidateIsNotOverlappingCourseSessions(course);
        }

        private void ValidateIsNotOverlappingStandaloneSessions(RepeatedSession course)
        {
            var standaloneSessions = GetAllStandaloneSessions();

            foreach (var standaloneSession in standaloneSessions)
            {
                if (course.IsOverlapping(standaloneSession))
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

        private List<StandaloneSession> GetAllStandaloneSessions()
        {
            var locations = BusinessRepository.GetAllLocations(BusinessId);
            var coaches = BusinessRepository.GetAllCoaches(BusinessId);
            var services = BusinessRepository.GetAllServices(BusinessId);

            var sessionDatas = BusinessRepository.GetAllStandaloneSessions(BusinessId);

            var sessions = new List<StandaloneSession>();
            foreach (var sessionData in sessionDatas)
            {
                var location = locations.Single(x => x.Id == sessionData.Location.Id);
                var coach = coaches.Single(x => x.Id == sessionData.Coach.Id);
                var service = services.Single(x => x.Id == sessionData.Service.Id);

                var session = new StandaloneSession(sessionData, location, coach, service);
                sessions.Add(session);
            }

            return sessions;
        }

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddSession((SessionAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            //if (ex is ClashingSession)
            //    return new ClashingSessionErrorResponse();

            return null;
        }
    }
}
