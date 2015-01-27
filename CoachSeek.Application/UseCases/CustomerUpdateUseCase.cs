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
    public class CustomerUpdateUseCase : UpdateUseCase<CustomerData>, ICustomerUpdateUseCase
    {
        public CustomerUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response<CustomerData> UpdateCustomer(CustomerUpdateCommand command)
        {
            return Update(command);
        }

        protected override CustomerData UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateCustomer((CustomerUpdateCommand)command, BusinessRepository);
        }

        protected override Response<CustomerData> HandleSpecificException(Exception ex)
        {
            if (ex is InvalidCustomer)
                return new InvalidCustomerUpdateResponse();
            //if (ex is DuplicateCoach)
            //    return new DuplicateCoachUpdateResponse();

            return null;
        }
    }
}