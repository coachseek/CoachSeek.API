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
    public class CoachUpdateUseCase : ICoachUpdateUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }

        public CoachUpdateUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public Response<CoachData> UpdateCoach(CoachUpdateCommand command)
        {
            if (command == null)
                return new NoCoachUpdateDataResponse();

            try
            {
                var business = GetBusiness(command);
                var coach = business.UpdateCoach(command, BusinessRepository);
                return new Response<CoachData>(coach);
            }
            catch (Exception ex)
            {
                return HandleUpdateCoachException(ex);
            }
        }

        private Business GetBusiness(CoachUpdateCommand command)
        {
            var business = BusinessRepository.Get(command.BusinessId);
            if (business == null)
                throw new InvalidBusiness();
            return business;
        }

        private Response<CoachData> HandleUpdateCoachException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();
            if (ex is InvalidCoach)
                return HandleInvalidCoach();
            if (ex is DuplicateCoach)
                return HandleDuplicateCoach();

            return null;
        }

        private Response<CoachData> HandleInvalidBusiness()
        {
            return new InvalidBusinessCoachUpdateResponse();
        }

        private Response<CoachData> HandleInvalidCoach()
        {
            return new InvalidCoachUpdateResponse();
        }

        private Response<CoachData> HandleDuplicateCoach()
        {
            return new DuplicateCoachUpdateResponse();
        }
    }
}