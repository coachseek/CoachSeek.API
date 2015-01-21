using System;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class LocationGetByIdUseCase : BaseUseCase<LocationData>, ILocationGetByIdUseCase
    {
        public Guid BusinessId { get; set; }

        public LocationGetByIdUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public LocationData GetLocation(Guid id)
        {
            var business = GetBusiness(BusinessId);
            return business.Locations.SingleOrDefault(x => x.Id == id);
        }
    }
}
