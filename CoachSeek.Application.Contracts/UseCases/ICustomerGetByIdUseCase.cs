using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerGetByIdUseCase : IApplicationContextSetter
    {
        CustomerData GetCustomer(Guid id);
    }
}
