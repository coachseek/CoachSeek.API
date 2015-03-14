using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IUserAssociateWithBusinessUseCase : IUserRepositorySetter
    {
        Response AssociateUserWithBusiness(UserAssociateWithBusinessCommand command);
    }
}
