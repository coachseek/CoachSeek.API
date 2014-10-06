using System;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;

namespace CoachSeek.WebUI.UseCases
{
    public class LocationUpdateUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public LocationUpdateUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }

        
        public LocationUpdateResponse UpdateLocation(LocationUpdateRequest request)
        {
            if (request == null)
                return new NoLocationUpdateDataResponse();

            try
            {
                var business = GetBusiness(request);
                var location = LocationConverter.Convert(request);
                business.UpdateLocation(location, BusinessRepository);
                return new LocationUpdateResponse(business);
            }
            catch (Exception ex)
            {
                return HandleUpdateLocationException(ex);
            }
        }

        private Business GetBusiness(LocationUpdateRequest request)
        {
            var business = BusinessRepository.Get(request.BusinessId);
            if (business == null)
                throw new InvalidBusinessException();
            return business;
        }

        private LocationUpdateResponse HandleUpdateLocationException(Exception ex)
        {
            if (ex is InvalidBusinessException)
                return HandleInvalidBusinessException();
            if (ex is InvalidLocationException)
                return HandleInvalidLocationException();
            if (ex is DuplicateLocationException)
                return HandleDuplicateLocationException();

            //if (ex is ValidationException)
            //    return new LocationAddResponse((ValidationException)ex);

            return null;
        }

        private LocationUpdateResponse HandleInvalidBusinessException()
        {
            return new InvalidBusinessLocationUpdateResponse();
        }

        private LocationUpdateResponse HandleInvalidLocationException()
        {
            return new InvalidLocationUpdateResponse();
        }

        private LocationUpdateResponse HandleDuplicateLocationException()
        {
            return new DuplicateLocationUpdateResponse();
        }
    }
}