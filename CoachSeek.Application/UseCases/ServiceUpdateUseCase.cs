using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class ServiceUpdateUseCase : UpdateUseCase<ServiceData>, IServiceUpdateUseCase
    {
        public ServiceUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<ServiceData> UpdateService(ServiceUpdateCommand command)
        {
            return Update(command);
        }

        protected override ServiceData UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateService((ServiceUpdateCommand)command, BusinessRepository);
        }

        protected override Response<ServiceData> HandleSpecificException(Exception ex)
        {
            if (ex is InvalidService)
                return new InvalidServiceUpdateResponse();
            if (ex is DuplicateService)
                return new DuplicateServiceUpdateResponse();

            return null;
        }
    }
}