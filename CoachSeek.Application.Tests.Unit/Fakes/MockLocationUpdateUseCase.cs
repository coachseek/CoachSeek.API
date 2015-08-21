using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class MockLocationUpdateUseCase : ILocationUpdateUseCase
    {
        public bool WasUpdateLocationCalled;
        public LocationUpdateCommand Command;
        public IResponse Response;

        public Guid BusinessId { set; get; }
        public IBusinessRepository BusinessRepository { get; set; }


        public void Initialise(ApplicationContext context)
        {
        }

        public IResponse UpdateLocation(LocationUpdateCommand command)
        {
            WasUpdateLocationCalled = true;
            Command = command;

            return Response;
        }
    }
}
