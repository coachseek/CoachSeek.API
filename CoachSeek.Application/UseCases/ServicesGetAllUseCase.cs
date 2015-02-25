using System;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class ServicesGetAllUseCase : BaseUseCase, IServicesGetAllUseCase
    {
        public Guid BusinessId { get; set; }

        public ServicesGetAllUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public IList<ServiceData> GetServices()
        {
            return BusinessRepository.GetAllServices(BusinessId);
        }
    }
}
