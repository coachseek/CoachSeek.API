using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationAddUseCase : IBusinessRepositorySetter
    {
        Response AddLocation(LocationAddCommand command);
    }
}
