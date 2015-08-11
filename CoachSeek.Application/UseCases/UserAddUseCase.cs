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
        public IUserRepository UserRepository { get; set; }


        public IResponse AddUser(UserAddCommand command)
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
            if (ex is SingleErrorException)
                return new ErrorResponse(ex as SingleErrorException);

            return null;
        }
    }
}
