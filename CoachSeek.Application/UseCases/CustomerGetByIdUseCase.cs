using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomerGetByIdUseCase : BaseUseCase, ICustomerGetByIdUseCase
    {
        public async Task<CustomerData> GetCustomerAsync(Guid id)
        {
            return await BusinessRepository.GetCustomerAsync(Business.Id, id);
        }
    }
}
