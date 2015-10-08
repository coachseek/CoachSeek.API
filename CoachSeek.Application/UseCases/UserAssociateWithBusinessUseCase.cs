using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class UserAssociateWithBusinessUseCase : IUserAssociateWithBusinessUseCase
    {
        public IUserRepository UserRepository { get; set; }


        public async Task<IResponse> AssociateUserWithBusinessAsync(UserAssociateWithBusinessCommand command)
        {
            var user = await UserRepository.GetAsync(command.UserId);
            SetBusinessAndRoleOnUser(user, command);
            await user.SaveAsync(UserRepository);
            return new Response(user.ToData());
        }

        private void SetBusinessAndRoleOnUser(User user, UserAssociateWithBusinessCommand command)
        {
            user.BusinessId = command.BusinessId;
            user.BusinessName = command.BusinessName;
            user.UserRole = command.Role;
        }
    }
}
