using System;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class CoachGetByIdUseCase : BaseUseCase<CoachData>, ICoachGetByIdUseCase
    {
        public Guid BusinessId { get; set; }

        public CoachGetByIdUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public CoachData GetCoach(Guid id)
        {
            var business = GetBusiness(BusinessId);
            return business.Coaches.SingleOrDefault(x => x.Id == id);
        }
    }
}
