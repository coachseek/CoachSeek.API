using System;
using System.Threading.Tasks;
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

        public async Task<IResponse> UpdateLocationAsync(LocationUpdateCommand command)
        {
            await Task.Delay(100);
            WasUpdateLocationCalled = true;
            Command = command;
            return Response;
        }
    }
}
