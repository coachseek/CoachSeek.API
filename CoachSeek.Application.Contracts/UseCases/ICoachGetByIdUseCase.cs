using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachGetByIdUseCase : IApplicationContextSetter
    {
        CoachData GetCoach(Guid id);
    }
}
