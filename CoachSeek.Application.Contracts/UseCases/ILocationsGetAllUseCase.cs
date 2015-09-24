using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationsGetAllUseCase : IApplicationContextSetter
    {
        Task<IList<LocationData>> GetLocationsAsync();

        //IList<LocationData> GetLocations();
    }
}
