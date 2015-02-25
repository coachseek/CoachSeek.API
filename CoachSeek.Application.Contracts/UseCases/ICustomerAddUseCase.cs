using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerAddUseCase
    {
        Guid BusinessId { set; }

        Response AddCustomer(CustomerAddCommand command);
    }
}
