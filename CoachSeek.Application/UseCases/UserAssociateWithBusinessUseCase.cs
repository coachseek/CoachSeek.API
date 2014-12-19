using System;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class UserAssociateWithBusinessUseCase : IUserAssociateWithBusinessUseCase
    {
        protected IUserRepository UserRepository { get; set; }

        public UserAssociateWithBusinessUseCase(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public Response<UserData> AssociateUserWithBusiness(UserAssociateWithBusinessCommand command)
        {
            //if (command == null)
            //    return new NoUserAddDataResponse();

            // Get user from repository
            var user = UserRepository.Get(command.UserId);

            // Fill in business info on user.
            user.BusinessId = command.BusinessId;
            user.BusinessName = command.BusinessName;

            // Update user.
            var updatedUser = user.Save(UserRepository);

            return new Response<UserData>(updatedUser);
        }
    }
}
