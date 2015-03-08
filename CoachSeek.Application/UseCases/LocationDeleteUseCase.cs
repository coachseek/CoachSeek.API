using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;

namespace CoachSeek.Application.UseCases
{
    public class LocationDeleteUseCase : BaseUseCase, ILocationDeleteUseCase
    {
        public Response DeleteLocation(Guid id)
        {
            var location = BusinessRepository.GetLocation(BusinessId, id);
            if (location == null)
                return new NotFoundResponse();

            // TODO: Delete the Location.

            return new Response(location);
        }
    }
}