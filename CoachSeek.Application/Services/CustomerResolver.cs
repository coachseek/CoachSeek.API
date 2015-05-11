using System;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Services
{
    public class CustomerResolver : ICustomerResolver
    {
        private Guid BusinessId { set; get; }
        private IBusinessRepository BusinessRepository { set; get; }


        public Customer Resolve(Customer searchCustomer)
        {
            var customers = BusinessRepository.GetAllCustomers(BusinessId);

            var matchingCustomerData = customers.SingleOrDefault(x => x.Email == searchCustomer.Email
                                                              && x.FirstName.ToLower() == searchCustomer.FirstName.ToLower()
                                                              && x.LastName.ToLower() == searchCustomer.LastName.ToLower());

            if (matchingCustomerData.IsNotFound())
                return null;

            return new Customer(matchingCustomerData);
        }

        public void Initialise(ApplicationContext context)
        {
            BusinessId = context.BusinessId.HasValue ? context.BusinessId.Value : Guid.Empty;
            BusinessRepository = context.BusinessRepository;
        }
    }
}
