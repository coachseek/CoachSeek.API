using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CoachGetByIdUseCase : BaseUseCase, ICoachGetByIdUseCase
    {
        public async Task<CoachData> GetCoachAsync(Guid id)
        {
            return await BusinessRepository.GetCoachAsync(Business.Id, id);
        }
    }
}
