using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class CoachUpdateUseCase : BaseUseCase, ICoachUpdateUseCase
    {
        public IResponse UpdateCoach(CoachUpdateCommand command)
        {
            try
            {
                var coach = new Coach(command);
                ValidateUpdate(coach);
                BusinessRepository.UpdateCoach(Business.Id, coach);
                return new Response(coach.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateUpdate(Coach coach)
        {
            var coaches = BusinessRepository.GetAllCoaches(Business.Id);

            var isExistingCoach = coaches.Any(x => x.Id == coach.Id);
            if (!isExistingCoach)
                throw new CoachInvalid(coach.Id);

            var existingCoach = coaches.FirstOrDefault(x => x.FirstName.ToLower() == coach.FirstName.ToLower()
                                                    && x.LastName.ToLower() == coach.LastName.ToLower());
            if (existingCoach != null && existingCoach.Id != coach.Id)
                throw new CoachDuplicate(coach.Name);
        }
    }
}