using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationUpdateUseCase : UpdateUseCase, ILocationUpdateUseCase
    {
        public Guid BusinessId { get; set; }

        
        public LocationUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response UpdateLocation(LocationUpdateCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var location = new Location(command);
                ValidateUpdate(location);
                var data = BusinessRepository.UpdateLocation(BusinessId, location);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is InvalidLocation)
                    return new InvalidLocationErrorResponse();
                if (ex is DuplicateLocation)
                    return new DuplicateLocationErrorResponse();
                throw;
            }
        }

        private void ValidateUpdate(Location location)
        {
            var locations = BusinessRepository.GetAllLocations(BusinessId);

            var isExistingLocation = locations.Any(x => x.Id == location.Id);
            if (!isExistingLocation)
                throw new InvalidLocation();

            var existingLocation = locations.FirstOrDefault(x => x.Name.ToLower() == location.Name.ToLower());
            if (existingLocation != null && existingLocation.Id != location.Id)
                throw new DuplicateLocation();
        }

        protected override object UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateLocation((LocationUpdateCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is InvalidLocation)
                return new InvalidLocationErrorResponse();
            if (ex is DuplicateLocation)
                return new DuplicateLocationErrorResponse();

            return null;
        }
    }
}