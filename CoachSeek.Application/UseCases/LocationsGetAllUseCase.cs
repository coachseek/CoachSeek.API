using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class LocationsGetAllUseCase : BaseUseCase, ILocationsGetAllUseCase
    {
        public Guid BusinessId { get; set; }

        public LocationsGetAllUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public IList<LocationData> GetLocations()
        {
            return BusinessRepository.GetAllLocations(BusinessId);
        }
    }
}
