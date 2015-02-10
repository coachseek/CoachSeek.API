using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BookingAddUseCase : AddUseCase<BookingData>, IBookingAddUseCase
    {
        private IBookingRepository BookingRepository { get; set; }

        public BookingAddUseCase(IBusinessRepository businessRepository,
            IBookingRepository bookingRepository)
            : base(businessRepository)
        {
            BookingRepository = bookingRepository;
        }


        public Response<BookingData> AddBooking(BookingAddCommand command)
        {
            return Add(command);
        }

        protected override BookingData AddToBusiness(Business business, IBusinessIdable command)
        {
            var bookingBusiness = business.BookingBusiness;
            return bookingBusiness.AddBooking((BookingAddCommand)command, BusinessRepository, BookingRepository);
        }

        protected override Response<BookingData> HandleSpecificException(Exception ex)
        {
            if (ex is InvalidSession)
                return new InvalidSessionResponse();

            return null;
        }
    }
}