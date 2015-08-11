using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Tests.Unit.Fakes
{
    public class StubCourseSessionBookingAddUseCase : ICourseSessionBookingAddUseCase
    {
        public bool WasCourseSet;
        public bool WasInitialiseCalled;
        public bool WasAddBookingCalled;

        public RepeatedSessionData Course
        {
            set { WasCourseSet = true; }
        }

        public void Initialise(ApplicationContext context)
        {
            WasInitialiseCalled = true;
        }

        public IResponse AddBooking(BookingAddCommand command)
        {
            WasAddBookingCalled = true;

            return new Response();
        }
    }
}
