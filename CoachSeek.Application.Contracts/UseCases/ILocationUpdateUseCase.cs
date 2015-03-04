using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationUpdateUseCase : IBusinessRepositorySetter
    {
        Response UpdateLocation(LocationUpdateCommand command);
    }
}
