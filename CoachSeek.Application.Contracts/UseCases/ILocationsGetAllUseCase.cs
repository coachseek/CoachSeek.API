using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationsGetAllUseCase : IApplicationContextSetter
    {
        IList<LocationData> GetLocations();
    }
}
