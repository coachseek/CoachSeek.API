using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceDeleteUseCase : IApplicationContextSetter
    {
        Task<Response> DeleteServiceAsync(Guid id);
    }
}
