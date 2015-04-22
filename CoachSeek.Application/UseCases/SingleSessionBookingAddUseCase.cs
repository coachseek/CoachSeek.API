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
                var data = BusinessRepository.AddSessionBooking(BusinessId, newBooking);
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

        private void ValidateAddBooking(SingleSessionBooking newBooking)
        {
            var errors = new ValidationException();

            ValidateBooking(newBooking, errors);

            ValidateAddBookingAdditional(newBooking, errors);

            errors.ThrowIfErrors();
        }


        protected virtual void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
        }

        protected virtual void ValidateAddBookingAdditional(SingleSessionBooking newBooking, ValidationException errors)
        {
        }


        private void ValidateCustomer(Guid customerId, ValidationException errors)
        {
            var customer = BusinessRepository.GetCustomer(BusinessId, customerId);
            if (customer.IsNotFound())
                errors.Add("This customer does not exist.", "booking.customer.id");
        }

        private void ValidateBooking(SingleSessionBooking newBooking, ValidationException errors)
        {
            var bookings = BusinessRepository.GetAllCustomerBookings(BusinessId);
            var isExistingBooking = bookings.Any(x => x.SessionId == newBooking.Session.Id
                                              && x.Customer.Id == newBooking.Customer.Id);
            if (isExistingBooking)
                throw new ValidationException("This customer is already booked for this session.");

            // TODO: Session or Course is full.
        }
    }
}