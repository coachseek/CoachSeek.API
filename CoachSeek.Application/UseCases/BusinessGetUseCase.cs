using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class BusinessGetUseCase : BaseUseCase, IBusinessGetUseCase
    {
        public async Task<BusinessData> GetBusinessAsync()
        {
            return await BusinessRepository.GetBusinessAsync(Business.Id);
        }
    }
}
