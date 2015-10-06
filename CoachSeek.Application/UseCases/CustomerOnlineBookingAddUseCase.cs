using System.Threading.Tasks;
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


        public async Task<IResponse> AddCustomerAsync(CustomerAddCommand command)
        {
            try
            {
                var onlineCustomer = new Customer(command);
                var matchingCustomer = await LookupCustomerAsync(onlineCustomer);
                if (matchingCustomer.IsFound())
                    return new Response(matchingCustomer);
                CustomerAddUseCase.Initialise(Context);
                return await CustomerAddUseCase.AddCustomerAsync(command);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }


        private async Task<Customer> LookupCustomerAsync(Customer onlineCustomer)
        {
            CustomerResolver.Initialise(Context);
            return await CustomerResolver.ResolveAsync(onlineCustomer);
        }
    }
}