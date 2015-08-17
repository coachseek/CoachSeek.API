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
        public IResponse AddLocation(LocationAddCommand command)
        {
            try
            {
                var newLocation = new Location(command);
                ValidateAdd(newLocation);
                var data = BusinessRepository.AddLocation(Business.Id, newLocation);
                return new Response(data);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateAdd(Location newLocation)
        {
            var locations = BusinessRepository.GetAllLocations(Business.Id);
            var isExistingLocation = locations.Any(x => x.Name.ToLower() == newLocation.Name.ToLower());
            if (isExistingLocation)
                throw new LocationDuplicate(newLocation.Name);
        }
    }
}