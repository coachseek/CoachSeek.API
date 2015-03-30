﻿using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Factories;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases.Factories
{
    public class BookingAddUseCaseFactory : IBookingAddUseCaseFactory
    {
        public Guid BusinessId { set; protected get; }

        public IBusinessRepository BusinessRepository { set; protected get; }


        public void Initialise(IBusinessRepository businessRepository, Guid? businessId = null)
        {
            BusinessId = businessId.HasValue ? businessId.Value : Guid.Empty;
            BusinessRepository = businessRepository;
        }
        

        public IBookingAddUseCase CreateUseCase(BookingAddCommand command)
        {
            var sessionOrCourse = GetExistingSessionOrCourse(command.Session.Id);
            if (sessionOrCourse.IsNotFound())
                throw new InvalidSession();

            var useCase = CreateSpecificUseCase(sessionOrCourse);
            InitialiseSpecificUseCase(useCase, sessionOrCourse);

            return useCase;
        }


        protected Session GetExistingSessionOrCourse(Guid sessionId)
        {
            // Is it a Session or a Course?
            var session = BusinessRepository.GetSession(BusinessId, sessionId);
            if (session.IsExisting())
            {
                if (session.ParentId == null)
                    return new StandaloneSession(session, LookupCoreData(session));

                return new SessionInCourse(session, LookupCoreData(session));
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

        private IBookingAddUseCase CreateSpecificUseCase(Session sessionOrCourse)
        {
            if (sessionOrCourse is SingleSession)
                return new SingleSessionBookingAddUseCase((SingleSession)sessionOrCourse);

            if (sessionOrCourse is RepeatedSession)
                return new CourseBookingAddUseCase((RepeatedSession)sessionOrCourse);

            throw new InvalidOperationException("Invalid session type.");
        }

        private void InitialiseSpecificUseCase(IBookingAddUseCase useCase, Session sessionOrCourse)
        {
            useCase.Initialise(BusinessRepository, BusinessId);
        }
    }
}
