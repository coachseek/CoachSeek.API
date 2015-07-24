using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingSetPaymentStatusUseCase : IApplicationContextSetter
    {
        Response SetPaymentStatus(BookingSetPaymentStatusCommand command);
    }
}
