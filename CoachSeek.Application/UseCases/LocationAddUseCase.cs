using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationAddUseCase : BaseUseCase, ILocationAddUseCase
    {
        public Response AddLocation(LocationAddCommand command)
        {
            try
            {
                var newLocation = new Location(command);
                ValidateAdd(newLocation);
                var data = BusinessRepository.AddLocation(BusinessId, newLocation);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is DuplicateLocation)
                    return new DuplicateLocationErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateAdd(Location newLocation)
        {
            var locations = BusinessRepository.GetAllLocations(BusinessId);
            var isExistingLocation = locations.Any(x => x.Name.ToLower() == newLocation.Name.ToLower());
            if (isExistingLocation)
                throw new DuplicateLocation();
        }
    }
}