using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.WebUI.Tests.Unit.Fakes
{
    public class MockLocationUpdateUseCase : ILocationUpdateUseCase
    {
        public bool WasUpdateLocationCalled;
        public LocationUpdateCommand Command;
        public Response Response;

        public Guid BusinessId { set; get; }
        public IBusinessRepository BusinessRepository { get; set; }

        public Response UpdateLocation(LocationUpdateCommand command)
        {
            WasUpdateLocationCalled = true;
            Command = command;

            return Response;
        }
    }
}
