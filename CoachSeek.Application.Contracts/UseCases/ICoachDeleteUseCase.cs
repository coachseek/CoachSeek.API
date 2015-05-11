using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachDeleteUseCase : IApplicationContextSetter
    {
        Response DeleteCoach(Guid id);
    }
}
