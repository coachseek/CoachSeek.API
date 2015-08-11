using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingSetPaymentStatusUseCase : IApplicationContextSetter
    {
        IResponse SetPaymentStatus(BookingSetPaymentStatusCommand command);
    }
}
