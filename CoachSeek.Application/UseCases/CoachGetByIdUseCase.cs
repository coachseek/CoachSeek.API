using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CoachGetByIdUseCase : BaseUseCase, ICoachGetByIdUseCase
    {
        public CoachData GetCoach(Guid id)
        {
            return BusinessRepository.GetCoach(Business.Id, id);
        }
    }
}
