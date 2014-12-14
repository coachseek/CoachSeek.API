using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockUserAddUseCase : IUserAddUseCase
    {
        public bool WasAddUserCalled;
        public UserAddCommand Command;
        public Response<UserData> Response;

        
        public Response<UserData> AddUser(UserAddCommand command)
        {
            WasAddUserCalled = true;

            Command = command;
            return Response;
        }
    }
}
