using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class ServiceGetByIdUseCase : BaseUseCase, IServiceGetByIdUseCase
    {
        public async Task<ServiceData> GetServiceAsync(Guid id)
        {
            return await BusinessRepository.GetServiceAsync(Business.Id, id);
        }
    }
}
