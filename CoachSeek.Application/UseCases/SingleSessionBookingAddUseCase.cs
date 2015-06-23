using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;

namespace CoachSeek.Application.UseCases
{
    public class SingleSessionBookingAddUseCase : SessionBaseUseCase, IBookingAddUseCase
    {
        protected SingleSession Session { get; set; }


        public SingleSessionBookingAddUseCase(SingleSession existingSession)
        {
            Session = existingSession;
        }


        public Response AddBooking(BookingAddCommand command)
        {
            try
            {
                ValidateCommand(command);
                var newBooking = new SingleSessionBooking(command);
                ValidateAddBooking(newBooking);
                var data = BusinessRepository.AddSessionBooking(Business.Id, newBooking);
                PostProcessing(newBooking);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is InvalidSession)
                    return new InvalidSessionErrorResponse("booking.session.id");
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }


        private void ValidateCommand(BookingAddCommand newBooking)
        {
            var errors = new ValidationException();

            ValidateCustomer(newBooking.Customer.Id, errors);
            ValidateCommandAdditional(newBooking, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateCustomer(Guid customerId, ValidationException errors)
        {
            var customer = BusinessRepository.GetCustomer(Business.Id, customerId);
            if (customer.IsNotFound())
                errors.Add("This customer does not exist.", "booking.customer.id");
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

        private void ValidateIsNewBooking(SingleSessionBooking newBooking)
        {
            var bookings = BusinessRepository.GetAllCustomerBookings(Business.Id);
            var isExistingBooking = bookings.Any(x => x.SessionId == newBooking.Session.Id
                                              && x.Customer.Id == newBooking.Customer.Id);
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