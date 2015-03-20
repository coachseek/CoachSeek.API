using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using System;
using System.Linq;

namespace CoachSeek.Application.UseCases
{
    public class BookingAddUseCase : BaseUseCase, IBookingAddUseCase
    {
        public Response AddBooking(BookingAddCommand command)
        {
            if (command == null)
                return new NoDataErrorResponse();

            try
            {
                var newBooking = new Booking(command);
                ValidateAdd(newBooking);
                var data = BusinessRepository.AddBooking(BusinessId, newBooking);
                return new Response(data);
            }
            catch (Exception ex)
            {
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidateAdd(Booking newBooking)
        {
            var errors = new ValidationException();

            ValidateSession(newBooking, errors);
            ValidateCustomer(newBooking, errors);

            errors.ThrowIfErrors();

            ValidateBooking(newBooking, errors);
        }

        private void ValidateSession(Booking newBooking, ValidationException errors)
        {
            var session = BusinessRepository.GetSession(BusinessId, newBooking.Session.Id);
            if (!session.IsExisting())
                errors.Add("This session does not exist.", "booking.session.id");
        }

        private void ValidateCustomer(Booking newBooking, ValidationException errors)
        {
            var customer = BusinessRepository.GetCustomer(BusinessId, newBooking.Customer.Id);
            if (!customer.IsExisting())
                errors.Add("This customer does not exist.", "booking.customer.id");
        }

        private void ValidateBooking(Booking newBooking, ValidationException errors)
        {
            var bookings = BusinessRepository.GetAllBookings(BusinessId);
            var isExistingBooking = bookings.Any(x => x.Session.Id == newBooking.Session.Id
                                                      && x.Customer.Id == newBooking.Customer.Id);
            if (isExistingBooking)
                throw new ValidationException("This customer is already in this session.");

            // TODO: Session or Course is full.
        }
    }
}