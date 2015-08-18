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
        public IResponse AddSession(SessionAddCommand command)
        {
            try
            {
                var coreData = LookupCoreData(command);

                if (IsStandaloneSession(command, coreData.Service))
                    return CreateStandaloneSession(command, coreData);

                return CreateCourse(command, coreData);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private CoreData LookupCoreData(SessionAddCommand command)
        {
            var location = BusinessRepository.GetLocation(Business.Id, command.Location.Id);
            var coach = BusinessRepository.GetCoach(Business.Id, command.Coach.Id);
            var service = BusinessRepository.GetService(Business.Id, command.Service.Id);

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
            var data = BusinessRepository.AddSession(Business.Id, newSession);
            return new Response(data);
        }

        private Response CreateCourse(SessionAddCommand command, CoreData coreData)
        {
            var newCourse = new RepeatedSession(command, coreData);
            ValidateAdd(newCourse);
            var data = BusinessRepository.AddCourse(Business.Id, newCourse);
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
            var sessions = GetAllSessions();

            foreach (var session in sessions)
            {
                if (newSession.IsOverlapping(session))
                    throw new SessionClashing(session);
            }
        }

        private void ValidateAdd(RepeatedSession newCourse)
        {
            ValidateIsNotOverlapping(newCourse);
        }

        private void ValidateIsNotOverlapping(RepeatedSession course)
        {
            ValidateIsNotOverlappingSessions(course);
        }

        private void ValidateIsNotOverlappingSessions(RepeatedSession newCourse)
        {
            var sessions = GetAllSessions();

            foreach (var session in sessions)
            {
                if (newCourse.IsOverlapping(session))
                    throw new SessionClashing(session);
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
