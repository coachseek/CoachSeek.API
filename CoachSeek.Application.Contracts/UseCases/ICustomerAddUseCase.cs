using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerAddUseCase : IBusinessRepositorySetter
    {
        Response AddCustomer(CustomerAddCommand command);
    }
}
