using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachDeleteUseCase : IBusinessRepositorySetter
    {
        Response DeleteCoach(Guid id);
    }
}
