using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class SessionDeleteUseCase : SessionBaseUseCase, ISessionDeleteUseCase
    {
        public Response DeleteSession(Guid id)
        {
            var sessionOrCourse = GetExistingSessionOrCourse(id);
            if (sessionOrCourse == null)
                return new NotFoundResponse();

            try
            {
                if (sessionOrCourse is SingleSession)
                    TryDeleteSession((SingleSession)sessionOrCourse);
                else if (sessionOrCourse is RepeatedSession)
                    TryDeleteCourse((RepeatedSession)sessionOrCourse);

                return new Response();
            }
            catch (Exception ex)
            {
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }


        private void TryDeleteSession(SingleSession session)
        {
            var bookings = BusinessRepository.GetCustomerBookingsBySessionId(Business.Id, session.Id);
            if (bookings.Count > 0)
                throw new ValidationException("Cannot delete session as it has one or more bookings.");

            BusinessRepository.DeleteSession(Business.Id, session.Id);
        }

        private void TryDeleteCourse(RepeatedSession course)
        {
            ValidateDeleteCourse(course.ToData());
            BusinessRepository.DeleteCourse(Business.Id, course.Id);
        }

        private void ValidateDeleteCourse(RepeatedSessionData course)
        {
            var bookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            AddBookingsToCourse(course, bookings);

            if (HasCourseGotBookings(course))
                throw new ValidationException("Cannot delete course as it has one or more bookings.");
        }

        private bool HasCourseGotBookings(RepeatedSessionData course)
        {
            if (course.Booking.BookingCount > 0)
                return true;

            foreach(var session in course.Sessions)
                if (session.Booking.BookingCount > 0)
                    return true;

            return false;
        }

        private void AddBookingsToCourse(RepeatedSessionData course, IList<CustomerBookingData> bookings)
        {
            course.Booking.Bookings = bookings.Where(x => x.SessionId == course.Id).ToList();
            course.Booking.BookingCount = course.Booking.Bookings.Count;

            foreach (var session in course.Sessions)
            {
                session.Booking.Bookings = bookings.Where(x => x.SessionId == session.Id).ToList();
                session.Booking.BookingCount = session.Booking.Bookings.Count;
            }
        }
    }
}