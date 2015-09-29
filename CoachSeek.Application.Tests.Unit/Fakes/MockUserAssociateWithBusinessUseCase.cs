using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class MockUserAssociateWithBusinessUseCase : IUserAssociateWithBusinessUseCase
    {
        public bool WasAssociateUserWithBusinessCalled;
        public Exception Exception;
        public UserAssociateWithBusinessCommand Command;
        public IResponse Response;

        public IUserRepository UserRepository { get; set; }

        public async Task<IResponse> AssociateUserWithBusinessAsync(UserAssociateWithBusinessCommand command)
        {
            WasAssociateUserWithBusinessCalled = true;
            Command = command;
            await Task.Delay(1000);
            if (Exception != null)
                throw Exception;
            return Response;
        }
    }
}
