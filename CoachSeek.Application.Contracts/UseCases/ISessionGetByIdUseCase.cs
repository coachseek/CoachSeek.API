using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionGetByIdUseCase : IBusinessRepositorySetter
    {
        SessionData GetSession(Guid id);
    }
}
