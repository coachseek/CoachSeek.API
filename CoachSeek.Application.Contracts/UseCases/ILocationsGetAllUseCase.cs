using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationsGetAllUseCase
    {
        Guid BusinessId { get; set; }

        IList<LocationData> GetLocations();
    }
}
