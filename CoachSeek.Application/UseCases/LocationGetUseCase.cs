using System;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class LocationGetUseCase : BaseUseCase<LocationData>, ILocationGetUseCase
    {
        public Guid BusinessId { get; set; }

        public LocationGetUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public LocationData GetLocation(Guid id)
        {
            var business = GetBusiness(BusinessId);
            return business.Locations.SingleOrDefault(x => x.Id == id);
        }
    }
}
