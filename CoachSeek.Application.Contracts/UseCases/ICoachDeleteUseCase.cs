using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachDeleteUseCase : IApplicationContextSetter
    {
        Task<Response> DeleteCoachAsync(Guid id);
    }
}
