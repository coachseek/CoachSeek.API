using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CustomerAddUseCase : BaseUseCase, ICustomerAddUseCase
    {
        public Response AddCustomer(CustomerAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newCustomer = new Customer(command);
                ValidateAdd(newCustomer);
                var data = BusinessRepository.AddCustomer(BusinessId, newCustomer);
                return new Response(data);
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

        private void ValidateAdd(Customer newCustomer)
        {
            //var customers = BusinessRepository.GetAllCustomers(BusinessId);
            //var isExistingCustomer = customers.Any(x => x.FirstName.ToLower() == newCoach.FirstName.ToLower()
            //                                    && x.LastName.ToLower() == newCoach.LastName.ToLower());
            //if (isExistingCustomer)
            //    throw new DuplicateCustomer();
        }
    }
}