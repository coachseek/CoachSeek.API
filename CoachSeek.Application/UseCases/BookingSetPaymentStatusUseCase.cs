using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class BookingSetPaymentStatusUseCase : BaseUseCase, IBookingSetPaymentStatusUseCase
    {
        private IBookingGetByIdUseCase BookingGetByIdUseCase { get; set; }

        public BookingSetPaymentStatusUseCase(IBookingGetByIdUseCase bookingGetByIdUseCase)
        {
            BookingGetByIdUseCase = bookingGetByIdUseCase;
        }


        public IResponse SetPaymentStatus(BookingSetPaymentStatusCommand command)
        {
            try
            {
                BookingGetByIdUseCase.Initialise(Context);
                var booking = BookingGetByIdUseCase.GetBooking(command.BookingId);
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