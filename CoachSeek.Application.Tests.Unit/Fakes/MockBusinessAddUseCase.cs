using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class MockBusinessAddUseCase : IBusinessAddUseCase
    {
        public bool WasAddBusinessCalled;
        public Exception Exception;
        public BusinessAddCommand Command;
        public IResponse Response;

        public Guid BusinessId { get; set; }
        public IBusinessRepository BusinessRepository { get; set; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { get; set; }


        public IResponse AddBusiness(BusinessAddCommand command)
        {
            WasAddBusinessCalled = true;
            Command = command;

            if (Exception != null)
                throw Exception;
            return Response;
        }
    }
}
