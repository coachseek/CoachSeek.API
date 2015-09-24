using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CoachesGetAllUseCase : BaseUseCase, ICoachesGetAllUseCase
    {
        public async Task<IList<CoachData>> GetCoachesAsync()
        {
            return await BusinessRepository.GetAllCoachesAsync(Business.Id);
        }
    }
}
