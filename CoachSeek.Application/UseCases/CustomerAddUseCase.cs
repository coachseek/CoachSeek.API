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
    public class CustomerAddUseCase : AddUseCase<CustomerData>, ICustomerAddUseCase
    {
        public CustomerAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<CustomerData> AddCustomer(CustomerAddCommand command)
        {
            return Add(command);
        }

        protected override CustomerData AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddCustomer((CustomerAddCommand)command, BusinessRepository);
        }

        protected override Response<CustomerData> HandleSpecificException(Exception ex)
        {
            //if (ex is DuplicateCoach)
            //    return new DuplicateCoachAddResponse();

            return null;
        }
    }
}