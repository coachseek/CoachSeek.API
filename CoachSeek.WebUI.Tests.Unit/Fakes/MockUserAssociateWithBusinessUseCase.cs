using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockUserAssociateWithBusinessUseCase : IUserAssociateWithBusinessUseCase
    {
        public bool WasAssociateUserWithBusinessCalled;
        public UserAssociateWithBusinessCommand Command;
        public Response<UserData> Response;


        public Response<UserData> AssociateUserWithBusiness(UserAssociateWithBusinessCommand command)
        {
            WasAssociateUserWithBusinessCalled = true;
            Command = command;
            return Response;
        }
    }
}
