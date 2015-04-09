using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetAttendanceUseCase : BaseUseCase
    {
        public Response SetAttendance(BookingSetAttendanceCommand command)
        {
            try
            {
                var booking = BusinessRepository.GetSessionBooking(BusinessId, command.BookingId);
                
                booking.HasAttended = command.HasAttended;

                BusinessRepository.UpdateBooking(BusinessId, booking);

                return new Response();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}