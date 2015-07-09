using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class StandaloneSessionBookingAddUseCase : BookingAddUseCase, IStandaloneSessionBookingAddUseCase
    {
        public SingleSessionData Session { set; protected get; }


        public Response AddBooking(BookingAddCommand command)
        {
            try
            {
                ValidateCommand(command);
                var newBooking = new SingleSessionBooking(command.Sessions[0], command.Customer);
                ValidateAddBooking(newBooking);
                var data = BusinessRepository.AddSessionBooking(Business.Id, newBooking);
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

            ValidateSessionCount(newBooking.Sessions, errors);
            ValidateCustomer(newBooking.Customer.Id, errors);
            ValidateCommandAdditional(newBooking, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateSessionCount(IList<SessionKeyCommand> sessions, ValidationException errors)
        {
            if (sessions.Count > 1)
                errors.Add("Standalone sessions must be booked one at a time.", "booking.sessions");
        }

        protected virtual void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
        }

        private void ValidateAddBooking(SingleSessionBooking newBooking)
        {
            ValidateIsNewBooking(newBooking);
            ValidateSpacesAvailable();

            ValidateAddBookingAdditional(newBooking);
        }

        private IList<CustomerBookingData> GetSessionBookings(Guid sessionId)
        {
            var allBookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            return allBookings.Where(x => x.SessionId == sessionId).ToList();
        }

        private void ValidateIsNewBooking(SingleSessionBooking newBooking)
        {
            var isExistingBooking = Session.Booking.Bookings.Any(x => x.Customer.Id == newBooking.Customer.Id);
            if (isExistingBooking)
                throw new ValidationException("This customer is already booked for this session.");
        }

        private void ValidateSpacesAvailable()
        {
            if (Session.Booking.BookingCount >= Session.Booking.StudentCapacity)
                throw new ValidationException("This session is already fully booked.");
        }

        protected virtual void ValidateAddBookingAdditional(SingleSessionBooking newBooking)
        {
            // When overrides error they must throw a ValidationException.
        }

        protected virtual void PostProcessing(SingleSessionBooking newBooking)
        {
            // Nothing to do for a coach-initiated booking.
        }
    }
}