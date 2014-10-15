using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
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


        public CoachUpdateResponse UpdateCoach(CoachUpdateCommand command)
        {
            if (command == null)
                return new NoCoachUpdateDataResponse();

            try
            {
                var business = GetBusiness(command);
                business.UpdateCoach(command, BusinessRepository);
                return new CoachUpdateResponse(business);
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

        private CoachUpdateResponse HandleUpdateCoachException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();
            if (ex is InvalidCoach)
                return HandleInvalidCoach();
            if (ex is DuplicateCoach)
                return HandleDuplicateCoach();

            return null;
        }

        private CoachUpdateResponse HandleInvalidBusiness()
        {
            return new InvalidBusinessCoachUpdateResponse();
        }

        private CoachUpdateResponse HandleInvalidCoach()
        {
            return new InvalidCoachUpdateResponse();
        }

        private CoachUpdateResponse HandleDuplicateCoach()
        {
            return new DuplicateCoachUpdateResponse();
        }
    }
}