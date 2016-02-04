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
    public class CustomerAddUseCase : BaseUseCase, ICustomerAddUseCase
    {
        private ICustomerResolver CustomerResolver { get; set; }
        private ICustomerGetByIdUseCase CustomerGetByIdUseCase { get; set; }

        public CustomerAddUseCase(ICustomerResolver customerResolver,
                                  ICustomerGetByIdUseCase customerGetByIdUseCase)
        {
            CustomerResolver = customerResolver;
            CustomerGetByIdUseCase = customerGetByIdUseCase;
        }


        public async Task<IResponse> AddCustomerAsync(CustomerAddCommand command)
        {
            try
            {
                var newCustomer = new Customer(command);
                await ValidateAddAsync(newCustomer);
                await BusinessRepository.AddCustomerAsync(Business.Id, newCustomer);
                return new Response(await GetCustomerAsync(newCustomer.Id));
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateAddAsync(Customer newCustomer)
        {
            var existingCustomer = await LookupCustomerAsync(newCustomer);
            if (existingCustomer.IsFound())
                throw new CustomerDuplicate(newCustomer);
        }

        private async Task<CustomerData> LookupCustomerAsync(Customer newCustomer)
        {
            CustomerResolver.Initialise(Context);
            return await CustomerResolver.ResolveAsync(newCustomer);
        }

        private async Task<CustomerData> GetCustomerAsync(Guid customerId)
        {
            CustomerGetByIdUseCase.Initialise(Context);
            return await CustomerGetByIdUseCase.GetCustomerAsync(customerId);
        }
    }
}