using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetPaymentStatusUseCase : BaseUseCase
    {
        public Response SetPaymentStatus(BookingSetPaymentStatusCommand command)
        {
            var booking = BusinessRepository.GetSessionBooking(BusinessId, command.BookingId);
            booking.PaymentStatus = command.PaymentStatus;
            BusinessRepository.UpdateBooking(BusinessId, booking);
            return new Response();
        }
    }
}