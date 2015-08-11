using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingAddMasterUseCase : IApplicationContextSetter
    {
        IResponse AddBooking(BookingAddCommand command);
        IResponse AddOnlineBooking(BookingAddCommand command);
    }
}
