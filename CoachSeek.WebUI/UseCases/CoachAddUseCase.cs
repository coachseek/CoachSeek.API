using System;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.UseCases
{
    public class CoachAddUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public CoachAddUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }

        
        public CoachAddResponse AddCoach(CoachAddRequest request)
        {
            if (request == null)
                return new NoCoachAddDataResponse();

            try
            {
                var business = GetBusiness(request);
                var coach = CoachConverter.Convert(request);
                business.AddCoach(coach, BusinessRepository);
                return new CoachAddResponse(business);
            }
            catch (Exception ex)
            {
                return HandleAddCoachException(ex);
            }
        }

        private Business GetBusiness(CoachAddRequest request)
        {
            var business = BusinessRepository.Get(request.BusinessId);
            if (business == null)
                throw new InvalidBusinessException();
            return business;
        }

        private CoachAddResponse HandleAddCoachException(Exception ex)
        {
            if (ex is InvalidBusinessException)
                return HandleInvalidBusinessException();
            if (ex is DuplicateCoachException)
                return HandleDuplicateCoachException();

            return null;
        }

        private CoachAddResponse HandleInvalidBusinessException()
        {
            return new InvalidBusinessCoachAddResponse();
        }

        private CoachAddResponse HandleDuplicateCoachException()
        {
            return new DuplicateCoachAddResponse();
        }
    }
}