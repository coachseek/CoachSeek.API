using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace CoachSeek.Application.UseCases
{
    public class UserAddUseCase : IUserAddUseCase
    {
        protected IUserRepository UserRepository { get; set; }

        public UserAddUseCase(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public Response<UserData> AddUser(UserAddCommand command)
        {
            if (command == null)
                return new NoUserAddDataResponse();

            try
            {
                var newUser = new NewUser(command);
                var user = newUser.Save(UserRepository).Result;
                return new Response<UserData>(user);
            }
            catch (AggregateException ex)
            {
                return HandleUserAddException(ex);
            }
        }

        private Response<UserData> HandleUserAddException(AggregateException ex)
        {
            if (ex.InnerException is DuplicateUser)
                return HandleDuplicateUser();

            return null;
        }

        private Response<UserData> HandleDuplicateUser()
        {
            return new DuplicateUserAddDataResponse();
        }
    }

    public class NoUserAddDataResponse : Response<UserData>
    {
        public NoUserAddDataResponse()
        {
            Errors = new List<ErrorData> { new ErrorData("Missing user data.") };
        }
    }
}
