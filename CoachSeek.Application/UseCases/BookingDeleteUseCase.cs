using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using System;

namespace CoachSeek.Application.UseCases
{
    public class BookingDeleteUseCase : BaseUseCase, IBookingDeleteUseCase
    {
        public Response DeleteBooking(Guid id)
        {
            var booking = BusinessRepository.GetBooking(BusinessId, id);
            if (booking == null)
                return new NotFoundResponse();

            BusinessRepository.DeleteBooking(BusinessId, id);

            return new Response();
        }
    }
}