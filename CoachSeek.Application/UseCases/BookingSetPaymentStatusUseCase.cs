using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetPaymentStatusUseCase : BaseUseCase
    {
        public Response SetPaymentStatus(BookingSetPaymentStatusCommand command)
        {
            var booking = BusinessRepository.GetSessionBooking(Business.Id, command.BookingId);
            booking.PaymentStatus = command.PaymentStatus;
            BusinessRepository.UpdateBooking(Business.Id, booking);
            return new Response();
        }
    }
}