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


        public IResponse AssociateUserWithBusiness(UserAssociateWithBusinessCommand command)
        {
            var user = UserRepository.Get(command.UserId);
            SetBusinessDetailsOnUser(user, command);
            user.Save(UserRepository);
            return new Response();
        }

        private void SetBusinessDetailsOnUser(User user, UserAssociateWithBusinessCommand command)
        {
            user.BusinessId = command.BusinessId;
            user.BusinessName = command.BusinessName;
        }
    }
}
