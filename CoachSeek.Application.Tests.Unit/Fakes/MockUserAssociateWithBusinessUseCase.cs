using System;
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

        public IResponse AssociateUserWithBusiness(UserAssociateWithBusinessCommand command)
        {
            WasAssociateUserWithBusinessCalled = true;
            Command = command;

            if (Exception != null)
                throw Exception;
            return Response;
        }
    }
}
