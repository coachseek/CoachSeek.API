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
    public class ServiceAddUseCase : AddUseCase, IServiceAddUseCase
    {
        public Guid BusinessId { get; set; }


        public ServiceAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddService(ServiceAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newService = new NewService(command);
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

        private void ValidateAdd(NewService newService)
        {
            var services = BusinessRepository.GetAllServices(BusinessId);
            var isExistingService = services.Any(x => x.Name.ToLower() == newService.Name.ToLower());
            if (isExistingService)
                throw new DuplicateService();
        }

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddService((ServiceAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is DuplicateService)
                return new DuplicateServiceErrorResponse();

            return null;
        }
    }
}