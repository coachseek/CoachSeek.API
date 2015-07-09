﻿using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class StubStandaloneSessionBookingAddUseCase : IStandaloneSessionBookingAddUseCase
    {
        public bool WasSessionSet;
        public bool WasInitialiseCalled;
        public bool WasAddBookingCalled;

        public SingleSessionData Session
        {
            set { WasSessionSet = true; }
        }

        public void Initialise(ApplicationContext context)
        {
            WasInitialiseCalled = true;
        }

        public Response AddBooking(BookingAddCommand command)
        {
            WasAddBookingCalled = true;

            return new Response();
        }
    }
}
