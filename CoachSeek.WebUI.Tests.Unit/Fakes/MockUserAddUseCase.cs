using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockUserAddUseCase : IUserAddUseCase
    {
        public bool WasAddUserCalled;
        public UserAddCommand Command;
        public Response Response;

        public IUserRepository UserRepository { get; set; }
        
        public Response AddUser(UserAddCommand command)
        {
            WasAddUserCalled = true;

            Command = command;
            return Response;
        }
    }
}
