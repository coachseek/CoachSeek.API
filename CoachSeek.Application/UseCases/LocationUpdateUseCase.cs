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
        public IResponse UpdateLocation(LocationUpdateCommand command)
        {
            try
            {
                var location = new Location(command);
                ValidateUpdate(location);
                var data = BusinessRepository.UpdateLocation(Business.Id, location);
                return new Response(data);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateUpdate(Location location)
        {
            var locations = BusinessRepository.GetAllLocations(Business.Id);

            var isExistingLocation = locations.Any(x => x.Id == location.Id);
            if (!isExistingLocation)
                throw new LocationInvalid(location.Id);

            var existingLocation = locations.FirstOrDefault(x => x.Name.ToLower() == location.Name.ToLower());
            if (existingLocation != null && existingLocation.Id != location.Id)
                throw new LocationDuplicate(location.Name);
        }
    }
}