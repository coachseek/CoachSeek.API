using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Conversion;
using CoachSeek.WebUI.Models.UseCases.Requests;
using CoachSeek.WebUI.Models.UseCases.Responses;
using System;

namespace CoachSeek.WebUI.UseCases
{
    public class LocationUpdateUseCase : ILocationUpdateUseCase
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
                throw new InvalidBusiness();
            return business;
        }

        private LocationUpdateResponse HandleUpdateLocationException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();
            if (ex is InvalidLocation)
                return HandleInvalidLocation();
            if (ex is DuplicateLocation)
                return HandleDuplicateLocation();

            return null;
        }

        private LocationUpdateResponse HandleInvalidBusiness()
        {
            return new InvalidBusinessLocationUpdateResponse();
        }

        private LocationUpdateResponse HandleInvalidLocation()
        {
            return new InvalidLocationUpdateResponse();
        }

        private LocationUpdateResponse HandleDuplicateLocation()
        {
            return new DuplicateLocationUpdateResponse();
        }
    }
}