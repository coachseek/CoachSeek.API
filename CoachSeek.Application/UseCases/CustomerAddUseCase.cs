using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CustomerAddUseCase : AddUseCase, ICustomerAddUseCase
    {
        public CustomerAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response AddCustomer(CustomerAddCommand command)
        {
            return Add(command);
        }

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddCustomer((CustomerAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            //if (ex is DuplicateCoach)
            //    return new DuplicateCoachAddResponse();

            return null;
        }
    }
}