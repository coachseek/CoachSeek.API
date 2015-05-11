using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingAddMasterUseCase : IApplicationContextSetter
    {
        Response AddBooking(BookingAddCommand command);
        Response AddOnlineBooking(BookingAddCommand command);
    }
}
