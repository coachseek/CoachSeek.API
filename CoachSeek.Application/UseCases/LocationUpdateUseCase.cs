using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class LocationUpdateUseCase : BaseUseCase, ILocationUpdateUseCase
    {
        public Response UpdateLocation(LocationUpdateCommand command)
        {
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
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

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
    }
}