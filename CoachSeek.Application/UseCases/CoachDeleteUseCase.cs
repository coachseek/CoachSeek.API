using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using System;
using System.Threading.Tasks;

namespace CoachSeek.Application.UseCases
{
    public class CoachDeleteUseCase : BaseUseCase, ICoachDeleteUseCase
    {
        public async Task<Response> DeleteCoachAsync(Guid id)
        {
            var coach = await BusinessRepository.GetCoachAsync(Business.Id, id);
            if (coach.IsNotFound())
                return new NotFoundResponse();

            // TODO: Delete the Coach.

            return new Response();
        }
    }
}