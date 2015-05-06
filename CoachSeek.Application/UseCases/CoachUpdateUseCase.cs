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
        public Response UpdateCoach(CoachUpdateCommand command)
        {
            try
            {
                var coach = new Coach(command);
                ValidateUpdate(coach);
                var data = BusinessRepository.UpdateCoach(BusinessId, coach);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is InvalidCoach)
                    return new InvalidCoachErrorResponse();
                if (ex is DuplicateCoach)
                    return new DuplicateCoachErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateUpdate(Coach coach)
        {
            var coaches = BusinessRepository.GetAllCoaches(BusinessId);

            var isExistingCoach = coaches.Any(x => x.Id == coach.Id);
            if (!isExistingCoach)
                throw new InvalidCoach();

            var existingCoach = coaches.FirstOrDefault(x => x.FirstName.ToLower() == coach.FirstName.ToLower()
                                                    && x.LastName.ToLower() == coach.LastName.ToLower());
            if (existingCoach != null && existingCoach.Id != coach.Id)
                throw new DuplicateCoach();
        }
    }
}