using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CoachAddUseCase : AddUseCase, ICoachAddUseCase
    {
        public CoachAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddCoach(CoachAddCommand command)
        {
            return Add(command);
        }

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddCoach((CoachAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is DuplicateCoach)
                return new DuplicateCoachErrorResponse();

            return null;
        }
    }
}