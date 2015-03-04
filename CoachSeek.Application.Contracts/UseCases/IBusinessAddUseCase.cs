using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessAddUseCase : IBusinessRepositorySetter
    {
        Response AddBusiness(BusinessAddCommand command);
    }
}
