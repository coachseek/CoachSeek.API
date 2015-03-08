using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CoachDeleteUseCase : BaseUseCase, ICoachDeleteUseCase
    {
        public Response DeleteCoach(Guid id)
        {
            var coach = BusinessRepository.GetCoach(BusinessId, id);
            if (coach == null)
                return new NotFoundResponse();

            // TODO: Delete the Coach.

            return new Response(coach);
        }
    }
}