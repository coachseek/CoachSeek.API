using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionDeleteUseCase : IBusinessRepositorySetter
    {
        Response DeleteSession(Guid id);
    }
}
