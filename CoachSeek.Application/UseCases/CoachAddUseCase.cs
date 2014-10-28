using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CoachAddUseCase : AddUseCase<CoachData>, ICoachAddUseCase
    {
        public CoachAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<CoachData> AddCoach(CoachAddCommand command)
        {
            return Add(command);
        }

        protected override CoachData AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddCoach((CoachAddCommand)command, BusinessRepository);
        }

        protected override Response<CoachData> HandleSpecificException(Exception ex)
        {
            if (ex is DuplicateCoach)
                return new DuplicateCoachAddResponse();

            return null;
        }
    }
}