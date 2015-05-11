using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessAddUseCase : IApplicationContextSetter
    {
        Response AddBusiness(BusinessAddCommand command);
    }
}
