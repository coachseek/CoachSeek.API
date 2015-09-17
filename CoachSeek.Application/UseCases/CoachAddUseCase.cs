using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class CoachAddUseCase : BaseUseCase, ICoachAddUseCase
    {
        public IResponse AddCoach(CoachAddCommand command)
        {
            try
            {
                var newCoach = new Coach(command);
                ValidateAdd(newCoach);
                BusinessRepository.AddCoach(Business.Id, newCoach);
                return new Response(newCoach.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateAdd(Coach newCoach)
        {
            var coaches = BusinessRepository.GetAllCoaches(Business.Id);
            var isExistingCoach = coaches.Any(x => x.FirstName.ToLower() == newCoach.FirstName.ToLower()
                                                && x.LastName.ToLower() == newCoach.LastName.ToLower());
            if (isExistingCoach)
                throw new CoachDuplicate(newCoach.Name);

            // TODO: Check that the business subscription allows an additional coach.
            //if (!Business.IsAllowedAdditionalCoach(coaches))
            //    throw new CoachDisallowedBySubscription(newCoach.Name);
        }
    }
}