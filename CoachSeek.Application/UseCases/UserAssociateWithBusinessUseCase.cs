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
            SetBusinessDetailsOnUser(user, command);
            await user.SaveAsync(UserRepository);
            return new Response();
        }

        private void SetBusinessDetailsOnUser(User user, UserAssociateWithBusinessCommand command)
        {
            user.BusinessId = command.BusinessId;
            user.BusinessName = command.BusinessName;
        }
    }
}
