using System;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
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
                throw new InvalidBusinessException();
            return business;
        }

        private CoachUpdateResponse HandleUpdateCoachException(Exception ex)
        {
            if (ex is InvalidBusinessException)
                return HandleInvalidBusinessException();
            //if (ex is InvalidLocationException)
            //    return HandleInvalidLocationException();
            //if (ex is DuplicateLocationException)
            //    return HandleDuplicateLocationException();

            return null;
        }

        private CoachUpdateResponse HandleInvalidBusinessException()
        {
            return new InvalidBusinessCoachUpdateResponse();
        }
    }
}