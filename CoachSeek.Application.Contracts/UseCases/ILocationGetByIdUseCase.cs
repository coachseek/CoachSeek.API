using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationGetByIdUseCase : IApplicationContextSetter
    {
        Task<LocationData> GetLocationAsync(Guid id);
    }
}
