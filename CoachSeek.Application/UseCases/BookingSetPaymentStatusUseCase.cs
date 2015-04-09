using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetPaymentStatusUseCase : BaseUseCase
    {
        public Response SetPaymentStatus(BookingSetPaymentStatusCommand command)
        {
            try
            {
                var booking = BusinessRepository.GetSessionBooking(BusinessId, command.BookingId);

                booking.PaymentStatus = command.PaymentStatus;

                // update db

                return new Response();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}