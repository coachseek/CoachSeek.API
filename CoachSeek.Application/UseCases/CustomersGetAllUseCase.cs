using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IList<CustomerData>> GetCustomersAsync()
        {
            return await BusinessRepository.GetAllCustomersAsync(Business.Id);
        }
    }
}
