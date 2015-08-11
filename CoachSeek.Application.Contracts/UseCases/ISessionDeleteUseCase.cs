using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionDeleteUseCase : IApplicationContextSetter
    {
        IResponse DeleteSession(Guid id);
    }
}
