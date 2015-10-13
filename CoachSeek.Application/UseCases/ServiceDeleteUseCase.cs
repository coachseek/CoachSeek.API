using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using System;
using System.Threading.Tasks;

namespace CoachSeek.Application.UseCases
{
    public class ServiceDeleteUseCase : BaseUseCase, IServiceDeleteUseCase
    {
        public async Task<Response> DeleteServiceAsync(Guid id)
        {
            var service = await BusinessRepository.GetServiceAsync(Business.Id, id);
            if (service.IsNotFound())
                return new NotFoundResponse();

            // TODO: Delete the Service.

            return new Response();
        }
    }
}