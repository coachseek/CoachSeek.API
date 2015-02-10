using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class SessionUpdateUseCase : UpdateUseCase, ISessionUpdateUseCase
    {
        public SessionUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response UpdateSession(SessionUpdateCommand command)
        {
            return Update(command);
        }

        protected override object UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateSession((SessionUpdateCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is InvalidSession)
                return new InvalidSessionErrorResponse();
            if (ex is ClashingSession)
                return new ClashingSessionErrorResponse();

            return null;
        }
    }
}
