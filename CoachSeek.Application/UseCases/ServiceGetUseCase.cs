using System;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class ServiceGetUseCase : BaseUseCase<ServiceData>, IServiceGetUseCase
    {
        public Guid BusinessId { get; set; }

        public ServiceGetUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public ServiceData GetService(Guid id)
        {
            var business = GetBusiness(BusinessId);
            return business.Services.SingleOrDefault(x => x.Id == id);
        }
    }
}
