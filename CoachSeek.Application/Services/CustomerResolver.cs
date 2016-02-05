using System;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Services
{
    public class CustomerResolver : ICustomerResolver
    {
        private Guid BusinessId { set; get; }
        private IBusinessRepository BusinessRepository { set; get; }


        public async Task<CustomerData> ResolveAsync(Customer searchCustomer)
        {
            var customers = await BusinessRepository.GetAllCustomersAsync(BusinessId);

            var matchingCustomerData = customers.SingleOrDefault(x => x.Email == searchCustomer.Email
                                                              && x.FirstName.ToLower() == searchCustomer.FirstName.ToLower()
                                                              && x.LastName.ToLower() == searchCustomer.LastName.ToLower());

            if (matchingCustomerData.IsNotFound())
                return null;

            return matchingCustomerData;
        }

        public void Initialise(ApplicationContext context)
        {
            BusinessId = context.BusinessContext.Business.Id;
            BusinessRepository = context.BusinessContext.BusinessRepository;
        }
    }
}
