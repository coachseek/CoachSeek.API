using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionDeleteUseCase : IApplicationContextSetter
    {
        Task<IResponse> DeleteSessionAsync(Guid id);
    }
}
