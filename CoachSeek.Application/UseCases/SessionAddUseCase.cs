using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class SessionAddUseCase : AddUseCase, ISessionAddUseCase
    {
        public SessionAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddSession(SessionAddCommand command)
        {
            return Add(command);
        }

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddSession((SessionAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is ClashingSession)
                return new ClashingSessionErrorResponse();

            return null;
        }
    }
}
