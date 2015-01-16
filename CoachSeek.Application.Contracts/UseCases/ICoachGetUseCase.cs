using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICoachGetUseCase
    {
        Guid BusinessId { get; set; }

        CoachData GetCoach(Guid id);
    }
}
