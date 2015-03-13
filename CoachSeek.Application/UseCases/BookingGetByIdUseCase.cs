using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class BookingGetByIdUseCase : BaseUseCase, IBookingGetByIdUseCase
    {
        public BookingData GetBooking(Guid id)
        {
            return BusinessRepository.GetBooking(BusinessId, id);
        }
    }
}
