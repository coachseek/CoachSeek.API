using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class ServiceUpdateUseCase : BaseUseCase, IServiceUpdateUseCase
    {
        public IResponse UpdateService(ServiceUpdateCommand command)
        {
            try
            {
                var service = ValidateAndCreateService(command);
                BusinessRepository.UpdateService(Business.Id, service);
                return new Response(service.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private Service ValidateAndCreateService(ServiceUpdateCommand command)
        {
            var allServices = BusinessRepository.GetAllServices(Business.Id);
            var isExistingService = allServices.Any(x => x.Id == command.Id);
            if (!isExistingService)
                throw new ServiceInvalid(command.Id);
            var service = new Service(command);
            var existingService = allServices.FirstOrDefault(x => x.Name.ToLower() == service.Name.ToLower());
            if (existingService != null && existingService.Id != service.Id)
                throw new ServiceDuplicate(service.Name);
            return service;
        }
    }
}