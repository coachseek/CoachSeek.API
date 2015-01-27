using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerGetByIdUseCase
    {
        Guid BusinessId { get; set; }

        CustomerData GetCustomer(Guid id);
    }
}
