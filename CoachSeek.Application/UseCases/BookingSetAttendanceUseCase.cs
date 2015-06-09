using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetAttendanceUseCase : BaseUseCase
    {
        public Response SetAttendance(BookingSetAttendanceCommand command)
        {
            var booking = BusinessRepository.GetSessionBooking(Business.Id, command.BookingId);
            booking.HasAttended = command.HasAttended;
            BusinessRepository.UpdateBooking(Business.Id, booking);
            return new Response();
        }
    }
}