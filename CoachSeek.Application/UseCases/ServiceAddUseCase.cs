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
        public IResponse AddService(ServiceAddCommand command)
        {
            try
            {
                var newService = new Service(command);
                ValidateAdd(newService);
                var data = BusinessRepository.AddService(Business.Id, newService);
                return new Response(data);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateAdd(Service newService)
        {
            var services = BusinessRepository.GetAllServices(Business.Id);
            var isExistingService = services.Any(x => x.Name.ToLower() == newService.Name.ToLower());
            if (isExistingService)
                throw new DuplicateService(newService.Name);
        }
    }
}