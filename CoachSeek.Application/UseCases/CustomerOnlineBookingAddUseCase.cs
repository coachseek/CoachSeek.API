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
    public class CustomerOnlineBookingAddUseCase : BaseUseCase, ICustomerOnlineBookingAddUseCase
    {
        private ICustomerResolver CustomerResolver { get; set; }
        private ICustomerAddUseCase CustomerAddUseCase { get; set; }


        public CustomerOnlineBookingAddUseCase(ICustomerResolver customerResolver,
                                               ICustomerAddUseCase customerAddUseCase)
        {
            CustomerResolver = customerResolver;
            CustomerAddUseCase = customerAddUseCase;
        }


        public Response AddCustomer(CustomerAddCommand command)
        {
            try
            {
                var onlineCustomer = new Customer(command);
                var matchingCustomer = LookupCustomer(onlineCustomer);
                if (matchingCustomer.IsFound())
                    return new Response(matchingCustomer);
                return CustomerAddUseCase.AddCustomer(command);
            }
            catch (Exception ex)
            {
                if (ex is InvalidEmailAddressFormat)
                    return new InvalidEmailAddressFormatErrorResponse("customer.email");
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }


        private Customer LookupCustomer(Customer onlineCustomer)
        {
            CustomerResolver.Initialise(BusinessRepository, BusinessId);
            return CustomerResolver.Resolve(onlineCustomer);
        }
    }
}