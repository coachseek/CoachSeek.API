using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IUserAddUseCase : IUserRepositorySetter
    {
        Response AddUser(UserAddCommand command);
    }
}
