using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CoachesGetAllUseCase : BaseUseCase, ICoachesGetAllUseCase
    {
        public IList<CoachData> GetCoaches()
        {
            return BusinessRepository.GetAllCoaches(Business.Id);
        }
    }
}
