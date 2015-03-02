using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class CustomerAddUseCase : AddUseCase, ICustomerAddUseCase
    {
        public Guid BusinessId { get; set; }


        public CustomerAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


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

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            return business.AddCustomer((CustomerAddCommand)command, BusinessRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            //if (ex is DuplicateCoach)
            //    return new DuplicateCoachAddResponse();

            return null;
        }
    }
}