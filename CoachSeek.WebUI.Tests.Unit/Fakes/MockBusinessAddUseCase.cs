using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockBusinessAddUseCase : IBusinessAddUseCase
    {
        public bool WasAddBusinessCalled;
        public BusinessAddCommand Command;
        public Response Response;


        public Response AddBusiness(BusinessAddCommand command)
        {
            WasAddBusinessCalled = true;
            Command = command;
            return Response;
        }
    }
}
