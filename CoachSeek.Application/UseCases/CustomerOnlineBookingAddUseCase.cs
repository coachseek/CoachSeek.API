using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

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


        public IResponse AddCustomer(CustomerAddCommand command)
        {
            try
            {
                var onlineCustomer = new Customer(command);
                var matchingCustomer = LookupCustomer(onlineCustomer);
                if (matchingCustomer.IsFound())
                    return new Response(matchingCustomer);
                CustomerAddUseCase.Initialise(Context);
                return CustomerAddUseCase.AddCustomer(command);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private Customer LookupCustomer(Customer onlineCustomer)
        {
            CustomerResolver.Initialise(Context);
            return CustomerResolver.Resolve(onlineCustomer);
        }
    }
}