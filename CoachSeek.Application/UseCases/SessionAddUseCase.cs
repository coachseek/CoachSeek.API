using System;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class SessionAddUseCase : AddUseCase<SessionData>, ISessionAddUseCase
    {
        public SessionAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<SessionData> AddSession(SessionAddCommand command)
        {
            return Add(command);
        }

        protected override SessionData AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddSession((SessionAddCommand)command, BusinessRepository);
        }

        protected override Response<SessionData> HandleSpecificException(Exception ex)
        {
            if (ex is ClashingSession)
                return new ClashingSessionAddResponse();

            return null;
        }
    }
}
