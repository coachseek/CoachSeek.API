using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class CourseSessionOnlineBookingAddUseCase : ICourseSessionOnlineBookingAddUseCase
    {
        public RepeatedSessionData Course
        {
            set { throw new NotImplementedException(); }
        }

        public Response AddBooking(BookingAddCommand command)
        {
            throw new NotImplementedException();
        }

        public void Initialise(ApplicationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
