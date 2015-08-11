using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CourseSessionBookingAddUseCase : BookingAddUseCase, ICourseSessionBookingAddUseCase
    {
        public RepeatedSessionData Course { set; protected get; }


        public IResponse AddBooking(BookingAddCommand command)
        {
            try
            {
                ValidateCommand(command);
                var newBooking = CreateCourseBooking(command);
                ValidateAddBooking(newBooking);
                var data = BusinessRepository.AddCourseBooking(Business.Id, newBooking);
                PostProcessing(newBooking);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is InvalidSession)
                    return new InvalidSessionErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }


        private void ValidateCommand(BookingAddCommand newBooking)
        {
            var errors = new ValidationException();

            ValidateSessions(newBooking.Sessions, errors);
            ValidateCustomer(newBooking.Customer.Id, errors);

            ValidateCommandAdditional(newBooking, errors);

            errors.ThrowIfErrors();
        }

        protected virtual CourseBooking CreateCourseBooking(BookingAddCommand command)
        {
            return new CourseBooking(command, Course);
        }

        private void ValidateSessions(IList<SessionKeyCommand> commandSessions, ValidationException errors)
        {
            ValidateSessionsInCourse(commandSessions, errors);
            ValidateSessionsUnique(commandSessions, errors);
        }

        private void ValidateSessionsInCourse(IEnumerable<SessionKeyCommand> commandSessions, ValidationException errors)
        {
            foreach (var commandSession in commandSessions)
            {
                if (!Course.Sessions.Select(x => x.Id).Contains(commandSession.Id))
                {
                    errors.Add("One or more of the sessions is not in the course.", "booking.sessions");
                    return;
                }
            }
        }

        private void ValidateSessionsUnique(IEnumerable<SessionKeyCommand> commandSessions, ValidationException errors)
        {
            if (commandSessions.GroupBy(x => x.Id).SelectMany(grp => grp.Skip(1)).Any())
                errors.Add("Some sessions are duplicates.", "booking.sessions");
        }

        protected virtual void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
        }


        private void ValidateAddBooking(CourseBooking newBooking)
        {
            ValidateIsNewBooking(newBooking);
            ValidateSpacesAvailable(newBooking);

            ValidateAddBookingAdditional(newBooking);
        }

        private void ValidateIsNewBooking(CourseBooking newBooking)
        {
            var customerSessionBookings = BusinessRepository.GetAllCustomerBookings(Business.Id)
                                                            .Where(x => x.Customer.Id == newBooking.Customer.Id)
                                                            .Where(x => x.ParentId != null)
                                                            .ToList();

            foreach (var customerSessionBooking in customerSessionBookings)
                if (newBooking.SessionBookings.Select(x => x.Session.Id).Contains(customerSessionBooking.SessionId))
                    throw new ValidationException("This customer is already booked onto a session in this course.");
        }

        private void ValidateSpacesAvailable(CourseBooking newBooking)
        {
            foreach (var sessionBookings in newBooking.SessionBookings)
            {
                var session = Course.Sessions.First(x => x.Id == sessionBookings.Session.Id);
                if (session.Booking.BookingCount >= session.Booking.StudentCapacity)
                    throw new ValidationException("One or more of the sessions is already fully booked.");
            }
        }

        protected virtual void ValidateAddBookingAdditional(CourseBooking newBooking)
        {
            // When overrides error they must throw a ValidationException.
        }

        protected virtual void PostProcessing(CourseBooking newBooking)
        {
            // Nothing to do for a coach-initiated booking.
        }
    }
}
