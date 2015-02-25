using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class ServiceUpdateUseCase : UpdateUseCase, IServiceUpdateUseCase
    {
        public Guid BusinessId { get; set; }

        
        public ServiceUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response UpdateService(ServiceUpdateCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var service = new Service(command);
                ValidateUpdate(service);
                var data = BusinessRepository.UpdateService(BusinessId, service);
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
            var services = BusinessRepository.GetAllServices(BusinessId);

            var isExistingService = services.Any(x => x.Id == service.Id);
            if (!isExistingService)
                throw new InvalidService();

            var existingService = services.FirstOrDefault(x => x.Name.ToLower() == service.Name.ToLower());
            if (existingService != null && existingService.Id != service.Id)
                throw new DuplicateService();
        }

        protected override object UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateService((ServiceUpdateCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is InvalidService)
                return new InvalidServiceErrorResponse();
            if (ex is DuplicateService)
                return new DuplicateServiceErrorResponse();

            return null;
        }
    }
}