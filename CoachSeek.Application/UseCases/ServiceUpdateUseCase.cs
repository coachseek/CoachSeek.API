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
        public ServiceUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response UpdateService(ServiceUpdateCommand command)
        {
            return Update(command);
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