using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockUserAssociateWithBusinessUseCase : IUserAssociateWithBusinessUseCase
    {
        public bool WasAssociateUserWithBusinessCalled;
        public UserAssociateWithBusinessCommand Command;
        public Response Response;


        public Response AssociateUserWithBusiness(UserAssociateWithBusinessCommand command)
        {
            WasAssociateUserWithBusinessCalled = true;
            Command = command;
            return Response;
        }
    }
}
