using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetPaymentStatusUseCase : BaseUseCase
    {
        public Response SetPaymentStatus(BookingSetPaymentStatusCommand command)
        {
            try
            {
                var booking = BusinessRepository.GetSessionBooking(Business.Id, command.BookingId);
                if (booking.IsNotFound())
                    return new NotFoundResponse();
                ValidatePaymentStatus(command.PaymentStatus);
                BusinessRepository.SetBookingPaymentStatus(Business.Id, booking.Id, command.PaymentStatus);
                return new Response();
            }
            catch (Exception ex)
            {
                if (ex is InvalidPaymentStatus)
                    return new InvalidPaymentStatusErrorResponse();
                if (ex is ValidationException)
                    return new ErrorResponse((ValidationException)ex);

                throw;
            }
        }

        private void ValidatePaymentStatus(string paymentStatus)
        {
            if (paymentStatus == Constants.PAYMENT_STATUS_PENDING_INVOICE ||
                paymentStatus == Constants.PAYMENT_STATUS_PENDING_PAYMENT ||
                paymentStatus == Constants.PAYMENT_STATUS_OVERDUE_PAYMENT ||
                paymentStatus == Constants.PAYMENT_STATUS_PAID)
                return;

            throw new InvalidPaymentStatus();
        }
    }
}