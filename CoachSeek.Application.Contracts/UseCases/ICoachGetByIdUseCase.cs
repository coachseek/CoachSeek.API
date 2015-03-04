using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachGetByIdUseCase : IBusinessRepositorySetter
    {
        CoachData GetCoach(Guid id);
    }
}
