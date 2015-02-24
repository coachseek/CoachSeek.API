using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class CoachesGetAllUseCase : BaseUseCase, ICoachesGetAllUseCase
    {
        public Guid BusinessId { get; set; }

        public CoachesGetAllUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }

        public IList<CoachData> GetCoaches()
        {
            return BusinessRepository.GetAllCoaches(BusinessId);
        }
    }
}
