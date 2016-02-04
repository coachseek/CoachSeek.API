using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CustomerOnlineBookingAddUseCase : BaseUseCase, ICustomerOnlineBookingAddUseCase
    {
        private ICustomerResolver CustomerResolver { get; set; }
        private ICustomerGetByIdUseCase CustomerGetByIdUseCase { get; set; }
        private ICustomerAddUseCase CustomerAddUseCase { get; set; }


        public CustomerOnlineBookingAddUseCase(ICustomerResolver customerResolver,
                                               ICustomerGetByIdUseCase customerGetByIdUseCase,
                                               ICustomerAddUseCase customerAddUseCase)
        {
            CustomerResolver = customerResolver;
            CustomerGetByIdUseCase = customerGetByIdUseCase;
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


        private async Task<CustomerData> LookupCustomerAsync(Customer onlineCustomer)
        {
            CustomerResolver.Initialise(Context);
            var resolvedCustomer = await CustomerResolver.ResolveAsync(onlineCustomer);
            if (resolvedCustomer.IsNotFound())
                return null;
            return await GetCustomerAsync(resolvedCustomer.Id);
        }

        private async Task<CustomerData> GetCustomerAsync(Guid customerId)
        {
            CustomerGetByIdUseCase.Initialise(Context);
            return await CustomerGetByIdUseCase.GetCustomerAsync(customerId);
        }
    }
}