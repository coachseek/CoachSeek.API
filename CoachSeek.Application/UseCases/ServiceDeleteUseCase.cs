using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;

namespace CoachSeek.Application.UseCases
{
    public class ServiceDeleteUseCase : BaseUseCase, IServiceDeleteUseCase
    {
        public Response DeleteService(Guid id)
        {
            var service = BusinessRepository.GetService(Business.Id, id);
            if (service == null)
                return new NotFoundResponse();

            // TODO: Delete the Service.

            return new Response(service);
        }
    }
}