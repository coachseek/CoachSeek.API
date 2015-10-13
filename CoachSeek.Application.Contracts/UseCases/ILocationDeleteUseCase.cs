using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationDeleteUseCase : IApplicationContextSetter
    {
        Task<Response> DeleteLocationAsync(Guid id);
    }
}
