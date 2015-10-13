using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class LocationUpdateUseCase : BaseUseCase, ILocationUpdateUseCase
    {
        public async Task<IResponse> UpdateLocationAsync(LocationUpdateCommand command)
        {
            try
            {
                var location = new Location(command);
                await ValidateUpdateAsync(location);
                await BusinessRepository.UpdateLocationAsync(Business.Id, location);
                return new Response(location.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateUpdateAsync(Location location)
        {
            var locations = await BusinessRepository.GetAllLocationsAsync(Business.Id);

            var isExistingLocation = locations.Any(x => x.Id == location.Id);
            if (!isExistingLocation)
                throw new LocationInvalid(location.Id);

            var existingLocation = locations.FirstOrDefault(x => x.Name.ToLower() == location.Name.ToLower());
            if (existingLocation != null && existingLocation.Id != location.Id)
                throw new LocationDuplicate(location.Name);
        }
    }
}