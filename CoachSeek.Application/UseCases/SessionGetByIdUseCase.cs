using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.UseCases
{
    public class SessionGetByIdUseCase : SessionBaseUseCase, ISessionGetByIdUseCase
    {
        public SessionData GetSession(Guid id)
       { 
            var sessionOrCourse = GetExistingSessionOrCourse(id);
            if (sessionOrCourse.IsNotFound())
                return null;

            if (sessionOrCourse is SingleSession)
            {
                var session = ((SingleSession)sessionOrCourse).ToData();
                session.Booking.Bookings = BusinessRepository.GetCustomerBookingsBySessionId(BusinessId, session.Id);

                return session;
            }
            if (sessionOrCourse is RepeatedSession)
            {
                var course = ((RepeatedSession)sessionOrCourse).ToData();
                course.Booking.Bookings = BusinessRepository.GetCustomerBookingsByCourseId(BusinessId, course.Id);

                return course;
            }

            throw new InvalidOperationException("Unexpected session type!");
        }
    }
}
