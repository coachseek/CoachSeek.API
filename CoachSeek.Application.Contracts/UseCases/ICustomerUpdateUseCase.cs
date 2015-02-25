using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerUpdateUseCase
    {
        Guid BusinessId { set; }

        Response UpdateCustomer(CustomerUpdateCommand command);
    }
}
