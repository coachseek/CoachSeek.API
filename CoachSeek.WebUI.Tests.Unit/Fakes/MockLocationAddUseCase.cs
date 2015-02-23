using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationAddUseCase : ILocationAddUseCase
    {
        public bool WasAddLocationCalled;
        public LocationAddCommand Command;
        public Response Response;

        public Guid BusinessId { get; set; }


        public Response AddLocation(LocationAddCommand command)
        {
            WasAddLocationCalled = true;
            Command = command;

            return Response;
        }
    }
}
