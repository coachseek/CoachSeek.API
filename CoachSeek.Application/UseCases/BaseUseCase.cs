using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase : IBusinessRepositorySetter
    {
        public Guid BusinessId { set; protected get; }

        public IBusinessRepository BusinessRepository { set; protected get; }


        public void Initialise(IBusinessRepository businessRepository, Guid? businessId = null)
        {
            BusinessId = businessId.HasValue ? businessId.Value : Guid.Empty;
            BusinessRepository = businessRepository;
        }
    }
}
