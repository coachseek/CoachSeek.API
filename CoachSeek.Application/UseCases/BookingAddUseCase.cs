using System.Linq;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities.Booking;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BookingAddUseCase : BaseUseCase, IBookingAddUseCase
    {
        public Guid BusinessId { get; set; }


        public BookingAddUseCase(IBusinessRepository businessRepository)
            : base(businessRepository)
        { }


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
                if (ex is InvalidSession)
                    return new InvalidSessionErrorResponse("booking.session.id");
                if (ex is InvalidCustomer)
                    return new InvalidCustomerErrorResponse("booking.customer.id");

                throw;
            }
        }

        private void ValidateAdd(Booking newBooking)
        {
            ValidateSession(newBooking);
            ValidateCustomer(newBooking);
            ValidateBooking(newBooking);
        }

        private void ValidateSession(Booking newBooking)
        {
            var session = BusinessRepository.GetSession(BusinessId, newBooking.Session.Id);
            if (!session.IsExisting())
                throw new InvalidSession();
        }

        private void ValidateCustomer(Booking newBooking)
        {
            var customer = BusinessRepository.GetCustomer(BusinessId, newBooking.Customer.Id);
            if (!customer.IsExisting())
                throw new InvalidCustomer();
        }

        private void ValidateBooking(Booking newBooking)
        {
            var bookings = BusinessRepository.GetAllBookings(BusinessId);
            var isExistingBooking = bookings.Any(x => x.Session.Id == newBooking.Session.Id
                                                      && x.Customer.Id == newBooking.Customer.Id);
            if (isExistingBooking)
                throw new DuplicateBooking();
        }
    }
}