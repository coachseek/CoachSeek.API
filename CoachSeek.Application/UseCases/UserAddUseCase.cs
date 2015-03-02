using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class UserAddUseCase : IUserAddUseCase
    {
        protected IUserRepository UserRepository { get; set; }

        public UserAddUseCase(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public Response AddUser(UserAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newUser = new NewUser(command);
                var user = newUser.Save(UserRepository);
                return new Response(user);
            }
            catch (Exception ex)
            {
                return HandleUserAddException(ex);
            }
        }

        private ErrorResponse HandleUserAddException(Exception ex)
        {
            if (ex is DuplicateUser)
                return new DuplicateUserErrorResponse();

            return null;
        }
    }
}
