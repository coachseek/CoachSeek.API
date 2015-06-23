using CoachSeek.Application.Contracts.Models;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetAttendanceUseCase : BaseUseCase
    {
        public Response SetAttendance(BookingSetAttendanceCommand command)
        {
            var booking = BusinessRepository.GetSessionBooking(Business.Id, command.BookingId);
            if (booking.IsNotFound())
                return new NotFoundResponse();
            BusinessRepository.SetBookingAttendance(Business.Id, booking.Id, command.HasAttended);
            return new Response();

            //booking.HasAttended = command.HasAttended;
            //BusinessRepository.UpdateBooking(Business.Id, booking);
            //return new Response();
        }
    }
}