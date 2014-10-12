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
    public class LocationAddUseCase : ILocationAddUseCase
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
                throw new InvalidBusiness();
            return business;
        }

        private LocationAddResponse HandleAddLocationException(Exception ex)
        {
            if (ex is InvalidBusiness)
                return HandleInvalidBusiness();
            if (ex is DuplicateLocation)
                return HandleDuplicateLocation();

            return null;
        }

        private LocationAddResponse HandleInvalidBusiness()
        {
            return new InvalidBusinessLocationAddResponse();
        }

        private LocationAddResponse HandleDuplicateLocation()
        {
            return new DuplicateLocationAddResponse();
        }
    }
}