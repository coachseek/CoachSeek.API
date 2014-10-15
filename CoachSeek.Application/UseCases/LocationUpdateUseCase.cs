using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationUpdateUseCase : ILocationUpdateUseCase
    {
        private IBusinessRepository BusinessRepository { get; set; }


        public LocationUpdateUseCase(IBusinessRepository businessRepository)
        {
            BusinessRepository = businessRepository;
        }

        
        public LocationUpdateResponse UpdateLocation(LocationUpdateCommand request)
        {
            if (request == null)
                return new NoLocationUpdateDataResponse();

            try
            {
                var business = GetBusiness(request);
                business.UpdateLocation(request, BusinessRepository);
                return new LocationUpdateResponse(business);
            }
            catch (Exception ex)
            {
                return HandleUpdateLocationException(ex);
            }
        }

        private Business GetBusiness(LocationUpdateCommand request)
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