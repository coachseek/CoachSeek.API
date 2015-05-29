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


        public Response AddCustomer(CustomerAddCommand command)
        {
            try
            {
                var newCustomer = new Customer(command);
                ValidateAdd(newCustomer);
                var data = BusinessRepository.AddCustomer(Business.Id, newCustomer);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is InvalidEmailAddressFormat)
                    return new InvalidEmailAddressFormatErrorResponse("customer.email");
                if (ex is DuplicateCustomer)
                    return new DuplicateCustomerErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateAdd(Customer newCustomer)
        {
            var existingCustomer = LookupCustomer(newCustomer);
            if (existingCustomer.IsFound())
                throw new DuplicateCustomer();
        }

        private Customer LookupCustomer(Customer newCustomer)
        {
            CustomerResolver.Initialise(Context);
            return CustomerResolver.Resolve(newCustomer);
        }
    }
}