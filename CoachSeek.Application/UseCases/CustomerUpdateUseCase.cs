using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CustomerUpdateUseCase : UpdateUseCase, ICustomerUpdateUseCase
    {
        public Guid BusinessId { get; set; }

        
        public CustomerUpdateUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


        public Response UpdateCustomer(CustomerUpdateCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var customer = new Customer(command);
                ValidateUpdate(customer);
                var data = BusinessRepository.UpdateCustomer(BusinessId, customer);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is InvalidCustomer)
                    return new InvalidCustomerErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateUpdate(Customer customer)
        {
            var customers = BusinessRepository.GetAllCustomers(BusinessId);

            var isExistingCustomer = customers.Any(x => x.Id == customer.Id);
            if (!isExistingCustomer)
                throw new InvalidCustomer();

            //var existingCustomer = customers.FirstOrDefault(x => x.FirstName.ToLower() == customer.FirstName.ToLower()
            //                                        && x.LastName.ToLower() == customer.LastName.ToLower());
            //if (existingCustomer != null && existingCustomer.Id != customer.Id)
            //    throw new DuplicateCoach();
        }

        protected override object UpdateInBusiness(Business business, IBusinessIdable command)
        {
            return business.UpdateCustomer((CustomerUpdateCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is InvalidCustomer)
                return new InvalidCustomerErrorResponse();

            return null;
        }
    }
}