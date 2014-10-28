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
    public class CoachUpdateUseCase : UpdateUseCase<CoachData>, ICoachUpdateUseCase
    {
        public CoachUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<CoachData> UpdateCoach(CoachUpdateCommand command)
        {
            return Update(command);
        }

        protected override CoachData UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateCoach((CoachUpdateCommand)command, BusinessRepository);
        }

        protected override Response<CoachData> HandleSpecificException(Exception ex)
        {
            if (ex is InvalidCoach)
                return new InvalidCoachUpdateResponse();
            if (ex is DuplicateCoach)
                return new DuplicateCoachUpdateResponse();

            return null;
        }
    }
}