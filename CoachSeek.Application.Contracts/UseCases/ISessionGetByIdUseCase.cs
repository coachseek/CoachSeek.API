using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionGetByIdUseCase : IApplicationContextSetter
    {
        SessionData GetSession(Guid id);
    }
}
