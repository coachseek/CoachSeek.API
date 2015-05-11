using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockBusinessAddUseCase : IBusinessAddUseCase
    {
        public bool WasAddBusinessCalled;
        public BusinessAddCommand Command;
        public Response Response;

        public Guid BusinessId { get; set; }
        public IBusinessRepository BusinessRepository { get; set; }


        public void Initialise(ApplicationContext context)
        {
        }

        public Response AddBusiness(BusinessAddCommand command)
        {
            WasAddBusinessCalled = true;
            Command = command;
            return Response;
        }
    }
}
