using System;
using System.Linq;
using CoachSeek.Application.Contracts.Services;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Services
{
    public class CustomerResolver : ICustomerResolver
    {
        public Guid BusinessId { set; protected get; }

        public IBusinessRepository BusinessRepository { set; protected get; }



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

        public void Initialise(IBusinessRepository businessRepository, Guid? businessId = null)
        {
            BusinessId = businessId.HasValue ? businessId.Value : Guid.Empty;
            BusinessRepository = businessRepository;
        }
    }
}
