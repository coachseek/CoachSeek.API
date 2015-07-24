using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class StubBookingGetByIdUseCase : IBookingGetByIdUseCase
    {
        public bool WasInitialiseCalled;
        public bool WasGetBookingCalled;
        public BookingData SetBookingData;


        public StubBookingGetByIdUseCase()
        { }

        public StubBookingGetByIdUseCase(BookingData setBookingData)
        {
            SetBookingData = setBookingData;
        }


        public void Initialise(ApplicationContext context)
        {
            WasInitialiseCalled = true;
        }

        public BookingData GetBooking(Guid id)
        {
            WasGetBookingCalled = true;

            return SetBookingData;
        }
    }
}
