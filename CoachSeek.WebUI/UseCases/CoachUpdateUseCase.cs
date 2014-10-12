using System;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.UseCases
{
    public class CoachUpdateUseCase : ICoachUpdateUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }

        public CoachUpdateUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public CoachUpdateResponse UpdateCoach(CoachUpdateRequest request)
        {
            if (request == null)
                return new NoCoachUpdateDataResponse();

            try
            {
                var business = GetBusiness(request);
                business.UpdateCoach(request, BusinessRepository);
                return new CoachUpdateResponse(business);
            }
            catch (Exception ex)
            {
                return HandleUpdateCoachException(ex);
            }
        }

        private Business GetBusiness(CoachUpdateRequest request)
        {
            var business = BusinessRepository.Get(request.BusinessId);
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