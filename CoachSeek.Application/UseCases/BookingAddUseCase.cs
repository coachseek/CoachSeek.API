using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BookingAddUseCase : AddUseCase, IBookingAddUseCase
    {
        private IBookingRepository BookingRepository { get; set; }

        public BookingAddUseCase(IBusinessRepository businessRepository,
            IBookingRepository bookingRepository)
            : base(businessRepository)
        {
            BookingRepository = bookingRepository;
        }


        public Response AddBooking(BookingAddCommand command)
        {
            return Add(command);
        }

        protected override object AddToBusiness(Business business, IBusinessIdable command)
        {
            var bookingBusiness = business.BookingBusiness;
            return bookingBusiness.AddBooking((BookingAddCommand)command, BusinessRepository, BookingRepository);
        }

        protected override ErrorResponse HandleSpecificException(Exception ex)
        {
            if (ex is InvalidSession)
                return new InvalidSessionErrorResponse("booking.session.id");
            if (ex is InvalidCustomer)
                return new InvalidCustomerErrorResponse("booking.customer.id");

            return null;
        }
    }
}