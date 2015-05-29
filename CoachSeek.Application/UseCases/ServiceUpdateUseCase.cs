using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class ServiceUpdateUseCase : BaseUseCase, IServiceUpdateUseCase
    {
        public Response UpdateService(ServiceUpdateCommand command)
        {
            try
            {
                var service = new Service(command);
                ValidateUpdate(service);
                var data = BusinessRepository.UpdateService(Business.Id, service);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is InvalidService)
                    return new InvalidServiceErrorResponse();
                if (ex is DuplicateService)
                    return new DuplicateServiceErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateUpdate(Service service)
        {
            var services = BusinessRepository.GetAllServices(Business.Id);

            var isExistingService = services.Any(x => x.Id == service.Id);
            if (!isExistingService)
                throw new InvalidService();

            var existingService = services.FirstOrDefault(x => x.Name.ToLower() == service.Name.ToLower());
            if (existingService != null && existingService.Id != service.Id)
                throw new DuplicateService();
        }
    }
}