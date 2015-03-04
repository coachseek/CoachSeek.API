using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerGetByIdUseCase : IBusinessRepositorySetter
    {
        CustomerData GetCustomer(Guid id);
    }
}
