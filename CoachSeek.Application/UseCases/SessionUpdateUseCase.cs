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
    public class SessionUpdateUseCase : UpdateUseCase<SessionData>, ISessionUpdateUseCase
    {
        public SessionUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<SessionData> UpdateSession(SessionUpdateCommand command)
        {
            return Update(command);
        }

        protected override SessionData UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateSession((SessionUpdateCommand)command, BusinessRepository);
        }

        protected override Response<SessionData> HandleSpecificException(Exception ex)
        {
            if (ex is InvalidSession)
                return new InvalidSessionUpdateResponse();
            if (ex is ClashingSession)
                return new ClashingSessionUpdateResponse();

            return null;
        }
    }
}
