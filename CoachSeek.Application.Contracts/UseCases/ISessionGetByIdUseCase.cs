using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionGetByIdUseCase : IApplicationContextSetter
    {
        Task<SessionData> GetSessionAsync(Guid id);
    }
}
