﻿using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class MockUserAddUseCase : IUserAddUseCase
    {
        public bool WasAddUserCalled;
        public Exception Exception;
        public UserAddCommand Command;
        public IResponse Response;

        public IUserRepository UserRepository { get; set; }
        
        public IResponse AddUser(UserAddCommand command)
        {
            WasAddUserCalled = true;
            Command = command;

            if (Exception != null)
                throw Exception;
            return Response;
        }
    }
}