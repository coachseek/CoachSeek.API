using System;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerGetByIdUseCase : IApplicationContextSetter
    {
        Task<CustomerData> GetCustomerAsync(Guid id);
    }
}
