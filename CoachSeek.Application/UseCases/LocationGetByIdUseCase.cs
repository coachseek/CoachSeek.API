using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class LocationGetByIdUseCase : BaseUseCase, ILocationGetByIdUseCase
    {
        public Guid BusinessId { get; set; }

        public LocationGetByIdUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public LocationData GetLocation(Guid id)
        {
            return BusinessRepository.GetLocation(BusinessId, id);
        }
    }
}
