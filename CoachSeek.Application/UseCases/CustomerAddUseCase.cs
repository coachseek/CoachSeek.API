using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CustomerAddUseCase : BaseUseCase, ICustomerAddUseCase
    {
        private ICustomerResolver CustomerResolver { get; set; }


        public CustomerAddUseCase(ICustomerResolver customerResolver)
        {
            CustomerResolver = customerResolver;
        }


        public IResponse AddCustomer(CustomerAddCommand command)
        {
            try
            {
                var newCustomer = new Customer(command);
                ValidateAdd(newCustomer);
                var data = BusinessRepository.AddCustomer(Business.Id, newCustomer);
                return new Response(data);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateAdd(Customer newCustomer)
        {
            var existingCustomer = LookupCustomer(newCustomer);
            if (existingCustomer.IsFound())
                throw new CustomerDuplicate(newCustomer);
        }

        private Customer LookupCustomer(Customer newCustomer)
        {
            CustomerResolver.Initialise(Context);
            return CustomerResolver.Resolve(newCustomer);
        }
    }
}