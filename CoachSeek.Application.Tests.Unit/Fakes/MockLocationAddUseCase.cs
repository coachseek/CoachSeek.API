using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class MockLocationAddUseCase : ILocationAddUseCase
    {
        public bool WasAddLocationCalled;
        public LocationAddCommand Command;
        public IResponse Response;

        public Guid BusinessId { get; set; }
        public IBusinessRepository BusinessRepository { get; set; }


        public void Initialise(ApplicationContext context)
        {
        }

        public async Task<IResponse> AddLocationAsync(LocationAddCommand command)
        {
            await Task.Delay(100);
            WasAddLocationCalled = true;
            Command = command;
            return Response;
        }
    }
}
