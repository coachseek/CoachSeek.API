using System;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.UseCases
{
    public class LocationAddUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public LocationAddUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }


        public LocationAddResponse AddLocation(LocationAddRequest request)
        {
            if (request == null)
                return new NoLocationAddDataResponse();

            try
            {
                var business = GetBusiness(request);
                var location = LocationConverter.Convert(request);
                business.AddLocation(location, BusinessRepository);
                return new LocationAddResponse(business);
            }
            catch (Exception ex)
            {
                return HandleAddLocationException(ex);
            }
        }

        private Business GetBusiness(LocationAddRequest request)
        {
            var business = BusinessRepository.Get(request.BusinessId);
            if (business == null)
                throw new InvalidBusinessException();
            return business;
        }

        private LocationAddResponse HandleAddLocationException(Exception ex)
        {
            if (ex is InvalidBusinessException)
                return HandleInvalidBusinessException();
            if (ex is DuplicateLocationException)
                return HandleDuplicateLocationException();

            //if (ex is ValidationException)
            //    return new LocationAddResponse((ValidationException)ex);

            return null;
        }

        private LocationAddResponse HandleInvalidBusinessException()
        {
            return new InvalidBusinessLocationAddResponse();
        }

        private LocationAddResponse HandleDuplicateLocationException()
        {
            return new DuplicateLocationAddResponse();
        }
    }
}