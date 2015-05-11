using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationAddUseCase : ILocationAddUseCase
    {
        public bool WasAddLocationCalled;
        public LocationAddCommand Command;
        public Response Response;

        public Guid BusinessId { get; set; }
        public IBusinessRepository BusinessRepository { get; set; }


        public void Initialise(ApplicationContext context)
        {
        }

        public Response AddLocation(LocationAddCommand command)
        {
            WasAddLocationCalled = true;
            Command = command;

            return Response;
        }
    }
}
