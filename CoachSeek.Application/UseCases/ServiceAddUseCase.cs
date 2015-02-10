using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class ServiceAddUseCase : AddUseCase, IServiceAddUseCase
    {
        public ServiceAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddService(ServiceAddCommand command)
        {
            return Add(command);
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