using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class CustomerUpdateUseCase : BaseUseCase, ICustomerUpdateUseCase
    {
        public async Task<IResponse> UpdateCustomerAsync(CustomerUpdateCommand command)
        {
            try
            {
                var customer = new Customer(command);
                await ValidateUpdateAsync(customer);
                await BusinessRepository.UpdateCustomerAsync(Business.Id, customer);
                return new Response(customer.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateUpdateAsync(Customer customer)
        {
            var customers = await BusinessRepository.GetAllCustomersAsync(Business.Id);

            var isExistingCustomer = customers.Any(x => x.Id == customer.Id);
            if (!isExistingCustomer)
                throw new CustomerInvalid(customer.Id);

            //var existingCustomer = customers.FirstOrDefault(x => x.FirstName.ToLower() == customer.FirstName.ToLower()
            //                                        && x.LastName.ToLower() == customer.LastName.ToLower());
            //if (existingCustomer != null && existingCustomer.Id != customer.Id)
            //    throw new DuplicateCoach();
        }
    }
}