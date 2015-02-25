using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class ServiceGetByIdUseCase : BaseUseCase, IServiceGetByIdUseCase
    {
        public Guid BusinessId { get; set; }

        public ServiceGetByIdUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public ServiceData GetService(Guid id)
        {
            return BusinessRepository.GetService(BusinessId, id);
        }
    }
}
