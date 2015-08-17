using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class CustomerUpdateUseCase : BaseUseCase, ICustomerUpdateUseCase
    {
        public IResponse UpdateCustomer(CustomerUpdateCommand command)
        {
            try
            {
                var customer = new Customer(command);
                ValidateUpdate(customer);
                var data = BusinessRepository.UpdateCustomer(Business.Id, customer);
                return new Response(data);
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateUpdate(Customer customer)
        {
            var customers = BusinessRepository.GetAllCustomers(Business.Id);

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