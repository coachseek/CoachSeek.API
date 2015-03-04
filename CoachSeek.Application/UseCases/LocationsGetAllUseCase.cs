using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class LocationsGetAllUseCase : BaseUseCase, ILocationsGetAllUseCase
    {
        public IList<LocationData> GetLocations()
        {
            return BusinessRepository.GetAllLocations(BusinessId);
        }
    }
}
