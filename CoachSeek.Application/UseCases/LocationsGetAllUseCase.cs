using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class LocationsGetAllUseCase : BaseUseCase, ILocationsGetAllUseCase
    {
        public async Task<IList<LocationData>> GetLocationsAsync()
        {
            return await BusinessRepository.GetAllLocationsAsync(Business.Id);
        }

        //public IList<LocationData> GetLocations()
        //{
        //    return BusinessRepository.GetAllLocations(Business.Id);
        //}
    }
}
