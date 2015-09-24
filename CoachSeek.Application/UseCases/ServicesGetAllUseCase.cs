using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class ServicesGetAllUseCase : BaseUseCase, IServicesGetAllUseCase
    {
        public async Task<IList<ServiceData>> GetServicesAsync()
        {
            return await BusinessRepository.GetAllServicesAsync(Business.Id);
        }
    }
}
