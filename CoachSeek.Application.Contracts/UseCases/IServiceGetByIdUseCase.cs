using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceGetByIdUseCase : IApplicationContextSetter
    {
        Task<ServiceData> GetServiceAsync(Guid id);
    }
}
