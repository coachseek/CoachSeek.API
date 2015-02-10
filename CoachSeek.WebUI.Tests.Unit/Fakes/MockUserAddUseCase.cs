using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockUserAddUseCase : IUserAddUseCase
    {
        public bool WasAddUserCalled;
        public UserAddCommand Command;
        public Response Response;

        
        public Response AddUser(UserAddCommand command)
        {
            WasAddUserCalled = true;

            Command = command;
            return Response;
        }
    }
}
