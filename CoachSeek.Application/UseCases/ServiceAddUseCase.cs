using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class ServiceAddUseCase : BaseUseCase, IServiceAddUseCase
    {
        public Response AddService(ServiceAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newService = new Service(command);
                ValidateAdd(newService);
                var data = BusinessRepository.AddService(BusinessId, newService);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is DuplicateService)
                    return new DuplicateServiceErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateAdd(Service newService)
        {
            var services = BusinessRepository.GetAllServices(BusinessId);
            var isExistingService = services.Any(x => x.Name.ToLower() == newService.Name.ToLower());
            if (isExistingService)
                throw new DuplicateService();
        }
    }
}