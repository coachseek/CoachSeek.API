using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class LocationGetByIdUseCase : BaseUseCase, ILocationGetByIdUseCase
    {
        public async Task<LocationData> GetLocationAsync(Guid id)
        {
            return await BusinessRepository.GetLocationAsync(Business.Id, id);
        }
    }
}
