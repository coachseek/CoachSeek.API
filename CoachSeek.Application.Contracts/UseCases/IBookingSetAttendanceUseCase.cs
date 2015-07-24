using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingSetAttendanceUseCase : IApplicationContextSetter
    {
        Response SetAttendance(BookingSetAttendanceCommand command);
    }
}
