using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BookingDeleteUseCase : BaseUseCase, IBookingDeleteUseCase
    {
        private IBookingGetByIdUseCase BookingGetByIdUseCase { get; set; }


        public BookingDeleteUseCase(IBookingGetByIdUseCase bookingGetByIdUseCase)
        {
            BookingGetByIdUseCase = bookingGetByIdUseCase;
        }


        public Response DeleteBooking(Guid id)
        {
            BookingGetByIdUseCase.Initialise(Context);
            var booking = BookingGetByIdUseCase.GetBooking(id);
            if (booking == null)
                return new NotFoundResponse();

            BusinessRepository.DeleteBooking(BusinessId, id);

            return new Response();
        }
    }
}