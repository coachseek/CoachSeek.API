using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class LocationAddUseCase : BaseUseCase, ILocationAddUseCase
    {
        public async Task<IResponse> AddLocationAsync(LocationAddCommand command)
        {
            try
            {
                var newLocation = new Location(command);
                await ValidateAddAsync(newLocation);
                await BusinessRepository.AddLocationAsync(Business.Id, newLocation);
                return new Response(newLocation.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateAddAsync(Location newLocation)
        {
            var locations = await BusinessRepository.GetAllLocationsAsync(Business.Id);
            var isExistingLocation = locations.Any(x => x.Name.ToLower() == newLocation.Name.ToLower());
            if (isExistingLocation)
                throw new LocationDuplicate(newLocation.Name);
        }
    }
}