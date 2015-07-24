using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetAttendanceUseCase : BaseUseCase, IBookingSetAttendanceUseCase
    {
        public Response SetAttendance(BookingSetAttendanceCommand command)
        {
            var booking = BusinessRepository.GetSessionBooking(Business.Id, command.BookingId);
            if (booking.IsNotFound())
                return new NotFoundResponse();
            BusinessRepository.SetBookingAttendance(Business.Id, booking.Id, command.HasAttended);
            return new Response();
        }
    }
}