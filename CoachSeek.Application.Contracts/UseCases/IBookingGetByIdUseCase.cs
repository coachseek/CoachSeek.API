using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingGetByIdUseCase : IApplicationContextSetter
    {
        BookingData GetBooking(Guid id);
    }
}
