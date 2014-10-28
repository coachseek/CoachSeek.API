using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class ServiceAddUseCase : BaseUseCase<ServiceData>
    {
        public ServiceAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<ServiceData> AddService(ServiceAddCommand command)
        {
            return Add(command);
        }

        protected override ServiceData AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddService((ServiceAddCommand)command, BusinessRepository);
        }

        protected override Response<ServiceData> HandleSpecificAddException(Exception ex)
        {
            if (ex is DuplicateService)
                return HandleDuplicateService();

            return null;
        }

        private Response<ServiceData> HandleDuplicateService()
        {
            return new DuplicateServiceAddResponse();
        }
    }
}