using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class ServicesGetAllUseCase : BaseUseCase<ServiceData>, IServicesGetAllUseCase
    {
        public Guid BusinessId { get; set; }

        public ServicesGetAllUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public IList<ServiceData> GetServices()
        {
            var business = GetBusiness(BusinessId);
            return business.Services.OrderBy(x => x.Name).ToList();
        }
    }
}
