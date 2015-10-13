using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;
using CoachSeek.Common.Extensions;

namespace CoachSeek.Application.UseCases
{
    public class LocationDeleteUseCase : BaseUseCase, ILocationDeleteUseCase
    {
        public async Task<Response> DeleteLocationAsync(Guid id)
        {
            var location = await BusinessRepository.GetLocationAsync(Business.Id, id);
            if (location.IsNotFound())
                return new NotFoundResponse();

            // TODO: Delete the Location.

            return new Response();
        }
    }
}