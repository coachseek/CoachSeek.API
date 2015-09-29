using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IUserAssociateWithBusinessUseCase : IUserRepositorySetter
    {
        Task<IResponse> AssociateUserWithBusinessAsync(UserAssociateWithBusinessCommand command);
    }
}
