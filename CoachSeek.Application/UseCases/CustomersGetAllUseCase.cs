using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomersGetAllUseCase : BaseUseCase, ICustomersGetAllUseCase
    {
        public IList<CustomerData> GetCustomers()
        {
            return BusinessRepository.GetAllCustomers(Business.Id);
        }
    }
}
